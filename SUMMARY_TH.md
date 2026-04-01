# รายงานการตรวจสอบ Code - ระบบประเมินผล PA
## สรุปผลการตรวจสอบและปัญหาการหายของข้อมูล

---

## 🔴 ปัญหาที่พบ (5 ข้อ)

### ปัญหา #1: ลบข้อมูลทั้งหมดทุกครั้งที่ Save (CRITICAL)
**ไฟล์**: frmPAService.aspx.vb - ฟังก์ชัน SavePAForm()  
**บรรทัด**: 713-735

**สาเหตุ**:
```vb
DELETE FROM tblPADetail WHERE username = '...'
DELETE FROM tblPAHeader WHERE username = '...'
-- แล้วค่อย INSERT ข้อมูลใหม่
```

**ผลกระทบ**:
- ทุกครั้งที่ user save จะลบข้อมูลเก่าทั้งหมด
- ถ้า boss save ระหว่างที่ user กำลัง edit → ข้อมูล boss หายไป
- ทำให้เกิด race condition ที่ user/boss data หายจากระบบ

**สถานการณ์เสี่ยง**:
```
1. User save ข้อมูลครั้งแรก → เก็บปกติ ✓
2. Boss save ข้อมูลของ boss
3. User edit และ save อีกครั้ง
   → DELETE ลบข้อมูล boss ไปด้วย!
4. Boss save อีกครั้ง
   → ไม่เจอข้อมูลเก่า → UPDATE ล้มเหลว
   → ข้อมูล boss หายเด็ด!
```

---

### ปัญหา #2: Boss Save เฉพาะ Comment แล้วลบข้อมูล User (CRITICAL)
**ไฟล์**: 
- frmFormPA.js - บรรทัด 126-158 (JavaScript ด้าน Client)
- frmPAService.aspx.vb - บรรทัด 750-771 (Server BossSave)

**สาเหตุ**:

**JavaScript ด้าน boss เก็บเพียง**:
```javascript
detail.comment = $('#txtPAComment_' + item_group_no + '_' + id).val();
// หายไป:
// - weight (น้ำหนัก)
// - KPI
// - STG
// - detail (รายละเอียด)
// - months (เดือนต่างๆ)
```

**Server ด้าน boss ทำอะไร**:
```vb
UPDATE tblPADetail SET comment = '...' 
WHERE item_no = '...'
-- ตรงนี้ต้องเจอ item_no เดียวกัน ถึงจะ UPDATE
```

**ผลกระทบ**:
- ถ้า item_no เปลี่ยน (user ลบแถว) → UPDATE ไม่เจอแถว
- User ที่บันทึก weight, KPI, STG จะหายไป
- ข้อมูล user ไม่ลบ แต่เป็น orphaned/incomplete

**ตัวอย่างการสูญเสียข้อมูล**:
```
User บันทึก:
- item_no=1: detail="Q1 Sales", weight=20, KPI=100, STG=80, comment=""
- item_no=2: detail="Q2 Service", weight=15, KPI=90, STG=70, comment=""

User ลบแถว 1 แล้ว save อีกครั้ง
- item_no=1: detail="Q2 Service", weight=15, KPI=90, STG=70 (เลขเปลี่ยนจาก 2)

Boss save ด้วย item_no=1
→ UPDATE tblPADetail SET comment='Good' WHERE item_no=1
→ แก้ "Q2 Service" แทน "Q1 Sales"!
→ Weight=20, KPI=100, STG=80 หายเด็ด!
```

---

### ปัญหา #3: Item.no Renumbering ทำให้ UPDATE ล้มเหลว (HIGH)
**ไฟล์**: frmFormPA.js + frmPAService.aspx.vb

**สาเหตุ**: ทั้ง client และ server มี logic ที่เลขแถว item_no นับใหม่ทุกครั้ง

```javascript
var new_id = 1;
for (id = 1; id <= rowCount; id++) {
    if (row.exists) {
        detail.item_no = new_id++; // ✗ เลขเปลี่ยน!
    }
}
```

ถ้า user:
- บันทึกแถว 5 อัน (item_no = 1,2,3,4,5)
- ลบแถว 2 และ 4
- บันทึกอีกครั้ง (item_no = 1,2,3 ≠ เดิม!)

Boss UPDATE จึงไม่เจอแถวที่ถูก (เพราะเลขไม่ตรง)

---

### ปัญหา #4: Boss Form ไม่เติมข้อมูล User ที่แก้ (HIGH)
**ไฟล์**: frmFormPA.js บรรทัด 126-139

**โค้ด**:
```javascript
var detail = new tblPADetail();
detail.username = username;
data.evaluate_year = evaluate_year;  // ❌ ควร detail.evaluate_year
data.round = round;                  // ❌ ควร detail.round

detail.item_group_no = item_group_no;
detail.item_no = new_id++;

detail.LoSBoss = $('#selPALoSBoss_' + item_group_no + '_' + id).val();  
detail.comment = $('#txtPAComment_' + item_group_no + '_' + id).val();
// หายไป: weight, KPI, STG, detail, months
```

**ปัญหา**:
- Line 3-4 ผิด (assign ให้ data แทน detail)
- ไม่เก็บข้อมูล user เพื่ออ้างอิง

---

### ปัญหา #5: BossSaveResult ทำเหมือนกัน (HIGH)
**ไฟล์**: frmPAService.aspx.vb บรรทัด 1282-1288

Boss save results แต่ตรงที่ UPDATE tblPADetail ยังมีปัญหา:
```vb
UPDATE tblPADetail SET comment = '...'
WHERE ... AND item_no = '...'
-- ถ้า item_no ไม่ตรง → ไม่อัปเดต → ข้อมูล user หายไป
```

---

## 📊 แผนภาพ Data Flow ปัจจุบัน (ผิด)

```
User save frmFormPA
  ↓
DELETE all tblPADetail
  ↓  
INSERT new tblPADetail ✓
  ↓
Boss save ใน frmFormPA
  ↓
UPDATE tblPADetail comment (อาจ fail!)
  ↓
User save อีกครั้ง
  ↓
DELETE all tblPADetail (รวม boss data!)
  ↓
INSERT user data ใหม่
  ↓
Boss data หายเด็ด! ❌
```

---

## ✅ วิธีแก้ไข

### ขั้น 1: ด่วน (1-2 ชั่วโมง) - ยับยั้งการหายข้อมูล
1. เปลี่ยนจาก DELETE+INSERT → UPDATE+INSERT
2. เพิ่ม error checking เมื่อ UPDATE
3. บันทึก deleted records แทนลบจริง

### ขั้น 2: หลัก (4-6 ชั่วโมง) - แก้ปัญหาลึก
1. สร้าง table แยกสำหรับ user vs boss
   - tblPADetail (user plan)
   - tblPADetailBoss (boss review)
2. ใช้ GUID หรือ field อื่น แทน item_no
3. ใช้ MERGE/UPSERT แทน DELETE+INSERT

### ขั้น 3: ถัดไป (1-2 วัน) - Concurrency Control
1. เพิ่ม timestamp และ version
2. ป้องกันการแก้ไขพร้อมกัน
3. เพิ่ม audit trail

---

## 📋 ไฟล์ที่ต้องแก้

| ไฟล์ | ฟังก์ชัน | ปัญหา | อগ่าน |
|------|---------|-------|--------|
| frmPAService.aspx.vb | SavePAForm | DELETE ทั้งหมด | 🔴 CRITICAL |
| frmPAService.aspx.vb | BossSave | UPDATE ล้มเหลว | 🔴 CRITICAL |
| frmFormPA.js | DoSaveAndConfirm | ไม่ขัดแบบ | 🟠 HIGH |
| frmFormPA.js | #btnBossSave | เก็บข้อมูลไม่ครบ | 🟠 HIGH |
| frmEvaluateFormPA.js | #btnBossSaveResult | เหมือน issue #2 | 🟠 HIGH |
| Database | Schema | ต้องเพิ่ม table | 🟠 HIGH |

---

## 🧪 Test Cases ที่ต้องทดสอบ

### Test 1: User → Boss → User Edit
```
1. User: บันทึก item 1 (weight=20, KPI=100)
2. Boss: บันทึก comment "ดี"
3. User: แก้ weight=25 และ save
4. ตรวจสอบ: 
   - weight ต้องเป็น 25 (ไม่กลับไป 20)
   - comment ของ boss ต้องยังอยู่
```

### Test 2: ลบแถว ไม่ให้ลืมข้อมูล
```
1. User: สร้าง 5 items
2. User: ลบ item 2 และ 4, save
3. Boss: บันทึก comments
4. ตรวจสอบ: Boss data ถูกต้องกับ items ที่ถูก
```

### Test 3: Concurrent Save
```
1. User แท็บที่ 1: เลื่อนไป save
2. Boss แท็บที่ 2: save ตอนเดียวกัน
3. ตรวจสอบ: ทั้งสองข้อมูลรักษาไว้ (ไม่กระแทก)
```

---

## 💾 ข้อมูลที่บันทึกไว้

ได้บันทึกรายการทั้งหมดไว้ใน 2 ไฟล์:

1. **CODE_REVIEW_FINDINGS.md** - รายการปัญหาโดยละเอียด พร้อมโค้ด
2. **IMPLEMENTATION_GUIDE.md** - ขั้นตอนแก้ไขทีละอัน พร้อม code sample

---

## ⏱️ Timeline แนะนำ

- **วันนี้**: Phase 1 (1-2 ชั่วโมง) - Quick fix
- **พรุ่งนี้**: Phase 2 (4-6 ชั่วโมง) - Core fix
- **2-3 วันแรก**: Phase 3 (1-2 วัน) - Full refactor

**รวม**: 2-3 วันเต็ม

---

## ⚠️ ความเสี่ยง ปัจจุบัน

- **ข้อมูล user หายเด็ด** ทุกครั้งที่ double edit
- **ข้อมูล boss หายเด็ด** เมื่อ user save ใหม่
- **ไม่มี audit trail** - ไม่รู้ว่า editor ใคร เมื่อไหร่
- **ไม่มี concurrency control** - race condition
- **Soft delete ไม่มี** - ลบจริงเลยไม่มี recovery

---

## 📞 ติดตามต่อ

อ่านรายละเอียดทั้งหมดใน:
- `CODE_REVIEW_FINDINGS.md` - รายการปัญหาสมบูรณ์
- `IMPLEMENTATION_GUIDE.md` - คำแนะนำการแก้ไขทีละขั้น

**พร้อมช่วยเมื่อใด:**
- ถาม Q&A เกี่ยวกับปัญหา
- Code review ก่อน push
- Test scenario ใดๆ
- Deploy และ verify
