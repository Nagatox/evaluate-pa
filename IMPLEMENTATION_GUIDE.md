# Implementation Guide - Data Loss Bug Fixes

## Quick Fix Priority Sequence

### Phase 1: Immediate Workaround (Today - prevents data loss)
**Time: 1-2 hours**

1. Replace DELETE+INSERT with conditional logic
2. Add error checking for silent UPDATE failures

### Phase 2: Core Fix (Next 1-2 days)
**Time: 4-6 hours**

1. Separate user and boss data into distinct tables
2. Use stable identifiers (not item_no)
3. Implement MERGE/UPSERT instead of DELETE+INSERT

### Phase 3: Full Refactor (Next 2-3 days)  
**Time: 1-2 days**

1. Add concurrency control (timestamps, versions)
2. Add comprehensive error handling
3. Test all scenarios

---

## Phase 1: Quick Workaround - 1-2 Hours

### Step 1.1: Change SavePAForm to Preserve Data

**File**: `frmPAService.aspx.vb` - Method `SavePAForm()`  
**Current Code** (lines 713-735): Uses DELETE

**Change To**:
```vb
Protected Sub SavePAForm(Optional ByVal bConfirm As Boolean = False)
    ' ... existing code ...
    
    Dim SQL As String
    Dim pConnection As ADODB.Connection = Nothing

    Try
        pConnection = ConnectDB()
        pConnection.BeginTrans()

        ' ✓ CHANGE: Instead of DELETE, use conditional INSERT/UPDATE
        
        ' Insert or update header
        If Not bConfirm Then
            ' Check if header exists
            SQL = "SELECT COUNT(*) FROM tblPAHeader " & _
                  "WHERE username = '" & username & "' " & _
                  "AND evaluate_year = '" & evaluate_year & "' " & _
                  "AND round = '" & round & "' "
            
            Dim rs As ADODB.Recordset = pConnection.Execute(SQL)
            Dim headerExists As Integer = rs.Fields(0).Value
            rs.Close()
            
            If headerExists > 0 Then
                ' UPDATE existing
                SQL = "UPDATE tblPAHeader SET " & _
                      "  suggest = '" & Str2SQL(objdata.suggest) & "' " & _
                      "  , user_entrydate = '" & objdata.user_entrydate & "' " & _
                      "WHERE username = '" & username & "' " & _
                      "AND evaluate_year = '" & evaluate_year & "' " & _
                      "AND round = '" & round & "' "
            Else
                ' INSERT new
                SQL = "INSERT INTO tblPAHeader (username, evaluate_year, round, user_entrydate, " & _
                      "boss, boss_entrydate, boss2, boss2_entrydate, suggest) " & _
                      "VALUES ('" & username & "', '" & evaluate_year & "', '" & round & "', " & _
                      "'" & objdata.user_entrydate & "', " & _
                      "'" & objdata.boss & "', '" & objdata.boss_entrydate & "', " & _
                      "'" & objdata.boss2 & "', '" & objdata.boss2_entrydate & "', " & _
                      "'" & Str2SQL(objdata.suggest) & "')"
            End If
        Else
            ' For confirm, still need to update
            SQL = "UPDATE tblPAHeader SET " & _
                  "confirmed = '1', " & _
                  "confirm_datetime = CONVERT(VARCHAR, GETDATE(),20), " & _
                  "suggest = '" & Str2SQL(objdata.suggest) & "' " & _
                  "WHERE username = '" & username & "' " & _
                  "AND evaluate_year = '" & evaluate_year & "' " & _
                  "AND round = '" & round & "' "
        End If
        
        pConnection.Execute(SQL)

        ' ❌ REMOVE: DELETE FROM tblPADetail - Don't delete!
        ' Instead, delete only records that are NO LONGER in the new data
        
        ' For simplicity in quick fix: Mark as deleted instead of deleting
        SQL = "UPDATE tblPADetail SET is_deleted = 1 " & _
              "WHERE username = '" & username & "' " & _
              "AND evaluate_year = '" & evaluate_year & "' " & _
              "AND round = '" & round & "' "
        ' First, you'll need to add this column:
        ' ALTER TABLE tblPADetail ADD is_deleted BIT DEFAULT 0
        
        pConnection.Execute(SQL)

        ' Now INSERT new records
        Dim i
        For i = 0 To objdata.recordCount - 1
            SQL = "INSERT INTO tblPADetail (username, evaluate_year, round, item_group_no, " & _
                  "item_no, detail, detail2, weight, month_08, month_09, month_10, " & _
                  "month_11, month_12, month_01, month_02, month_03, month_04, " & _
                  "month_05, month_06, month_07, KPI, STG, comment, " & _
                  "record_datetime, editor, is_deleted) " & _
                  "VALUES ('" & username & "', '" & evaluate_year & "', '" & round & "', " & _
                  "'" & objdata.detail(i).item_group_no & "', " & _
                  "'" & objdata.detail(i).item_no & "', " & _
                  "'" & Str2SQL(objdata.detail(i).detail) & "', " & _
                  "'" & Str2SQL(objdata.detail(i).detail2) & "', " & _
                  "'" & objdata.detail(i).weight & "', " & _
                  "'" & objdata.detail(i).month_08 & "', " & _
                  "'" & objdata.detail(i).month_09 & "', " & _
                  "'" & objdata.detail(i).month_10 & "', " & _
                  "'" & objdata.detail(i).month_11 & "', " & _
                  "'" & objdata.detail(i).month_12 & "', " & _
                  "'" & objdata.detail(i).month_01 & "', " & _
                  "'" & objdata.detail(i).month_02 & "', " & _
                  "'" & objdata.detail(i).month_03 & "', " & _
                  "'" & objdata.detail(i).month_04 & "', " & _
                  "'" & objdata.detail(i).month_05 & "', " & _
                  "'" & objdata.detail(i).month_06 & "', " & _
                  "'" & objdata.detail(i).month_07 & "', " & _
                  "'" & Str2SQL(objdata.detail(i).KPI) & "', " & _
                  "'" & objdata.detail(i).STG & "', " & _
                  "'" & Str2SQL(objdata.detail(i).comment) & "', " & _
                  "'" & objdata.detail(i).record_datetime & "', " & _
                  "'" & objdata.detail(i).editor & "', 0)"
            
            pConnection.Execute(SQL)
        Next i

        pConnection.CommitTrans()

    Catch ex As Exception
        objJSONHeader.isError = True
        objJSONHeader.ErrMessage = ex.Message
        pConnection.RollbackTrans()
    Finally
        CloseDB(pConnection)
        Dim jobjsend As JavaScriptSerializer = New JavaScriptSerializer()
        Response.Write(jobjsend.Serialize(objJSONHeader))
        Response.Flush()
        Response.End()
    End Try
End Sub
```

**Database Changes Needed**:
```sql
ALTER TABLE tblPADetail ADD is_deleted BIT DEFAULT 0;
ALTER TABLE tblPAHeader ADD is_deleted BIT DEFAULT 0;

-- Update your SELECT statements to filter out deleted records:
-- AND is_deleted = 0
```

### Step 1.2: Add Error Checking to BossSave

**File**: `frmPAService.aspx.vb` - Method `BossSave()`  
**Lines**: 750-771

**Add After Each Execute**:
```vb
For i = 0 To objdata.recordCount - 1
    SQL = "UPDATE tblPADetail " & _
          " SET comment = '" & Str2SQL(objdata.detail(i).comment) & "' " & _
          " WHERE username = '" & username & "' " & _
          "   AND evaluate_year = '" & evaluate_year & "' " & _
          "   AND round = '" & round & "' " & _
          "   AND item_group_no = '" & objdata.detail(i).item_group_no & "' " & _
          "   AND item_no = '" & objdata.detail(i).item_no & "' " & _
          ""
    
    ' ✓ ADD: Check if UPDATE actually updated anything
    Dim rowCount As Integer
    On Error Resume Next
    Dim recordsAffected As Object = pConnection.Execute(SQL, rowCount)
    On Error GoTo 0
    
    If rowCount = 0 Then
        ' Log warning but don't fail - data might have different structure
        ' TODO: Log to error table: item_no mismatch for user/year/round
        objJSONHeader.ErrMessage = "Warning: Row " & i & " not found (item_no mismatch)"
    End If
Next i
```

### Step 1.3: Require Both User AND Boss Fields

**File**: `frmFormPA.js` - Method `DoSaveAndConfirm()`  
**Lines**: 70-120

Add validation that both user's data and boss's comment are present:
```javascript
function DoSaveAndConfirm(service_name, prefix_msg) {
    // ... existing code ...
    
    var hasData = false;
    for (item_group_no = 1; item_group_no <= 5; item_group_no++) {
        for (id = 1; id <= gRowNumArray[item_group_no-1]; id++) {
            if ($('#txtPAWeight_' + item_group_no + '_' + id).val() !== undefined) {
                hasData = true;
                break;
            }
        }
        if (hasData) break;
    }
    
    if (!hasData) {
        alert('ไม่พบข้อมูลในแบบฟอร์ม');
        return false;
    }
    
    // ... rest of code ...
}
```

---

## Phase 2: Core Fix - 4-6 Hours

### Step 2.1: Create Separate Tables for User vs Boss

**Database Schema Changes**:
```sql
-- Create table for boss's comments (separate from user's plan)
CREATE TABLE tblPADetailBoss (
    id INT PRIMARY KEY IDENTITY(1,1),
    username NVARCHAR(100),
    evaluate_year NVARCHAR(20),
    round NVARCHAR(20),
    item_group_no INT,
    item_no INT,
    LoSBoss VARCHAR(10),
    comment NVARCHAR(MAX),
    record_datetime DATETIME DEFAULT GETDATE(),
    editor NVARCHAR(100),
    UNIQUE (username, evaluate_year, round, item_group_no, item_no)
);

-- Modify tblPADetail to prevent boss edits
ALTER TABLE tblPADetail DROP CONSTRAINT [FK_or_trigger_if_exists];

-- Add timestamps for concurrency
ALTER TABLE tblPADetail ADD (
    created_datetime DATETIME DEFAULT GETDATE(),
    last_modified_datetime DATETIME DEFAULT GETDATE(),
    version INT DEFAULT 1
);

ALTER TABLE tblPADetailBoss ADD (
    created_datetime DATETIME DEFAULT GETDATE(),
    last_modified_datetime DATETIME DEFAULT GETDATE(),
    version INT DEFAULT 1
);
```

### Step 2.2: Update BossSave to Use Separate Table

**File**: `frmPAService.aspx.vb` - Method `BossSave()`

```vb
Protected Sub BossSave()
    ' ... existing code ...
    
    Try
        pConnection = ConnectDB()
        pConnection.BeginTrans()

        ' Update header
        SQL = "UPDATE tblPAHeader " & _
              " SET suggest = '" & Str2SQL(objdata.suggest) & "' " & _
              " WHERE username = '" & username & "' " & _
              "   AND evaluate_year = '" & evaluate_year & "' " & _
              "   AND round = '" & round & "' "
        pConnection.Execute(SQL)

        ' ✓ CHANGE: Use separate tblPADetailBoss table
        For i = 0 To objdata.recordCount - 1
            ' First, collect FULL data for boss to preserve
            SQL = "SELECT detail, detail2, weight, KPI, STG FROM tblPADetail " & _
                  " WHERE username = '" & username & "' " & _
                  " AND evaluate_year = '" & evaluate_year & "' " & _
                  " AND round = '" & round & "' " & _
                  " AND item_group_no = '" & objdata.detail(i).item_group_no & "' " & _
                  " AND item_no = '" & objdata.detail(i).item_no & "' "
            
            Dim rs As ADODB.Recordset = pConnection.Execute(SQL)
            Dim userDetail As String = ""
            Dim userDetail2 As String = ""
            Dim userWeight As String = ""
            Dim userKPI As String = ""
            Dim userSTG As String = ""
            
            If Not rs.EOF Then
                userDetail = rs.Fields("detail").Value
                userDetail2 = rs.Fields("detail2").Value
                userWeight = rs.Fields("weight").Value
                userKPI = rs.Fields("KPI").Value
                userSTG = rs.Fields("STG").Value
            End If
            rs.Close()
            
            ' Insert/Update boss comment in separate table
            SQL = "IF EXISTS (SELECT 1 FROM tblPADetailBoss " & _
                  "WHERE username = '" & username & "' " & _
                  "AND evaluate_year = '" & evaluate_year & "' " & _
                  "AND round = '" & round & "' " & _
                  "AND item_group_no = '" & objdata.detail(i).item_group_no & "' " & _
                  "AND item_no = '" & objdata.detail(i).item_no & "') " & _
                  "UPDATE tblPADetailBoss SET " & _
                  "  LoSBoss = '" & objdata.detail(i).LoSBoss & "' " & _
                  "  , comment = '" & Str2SQL(objdata.detail(i).comment) & "' " & _
                  "  , last_modified_datetime = GETDATE() " & _
                  "  , version = version + 1 " & _
                  "WHERE username = '" & username & "' " & _
                  "AND evaluate_year = '" & evaluate_year & "' " & _
                  "AND round = '" & round & "' " & _
                  "AND item_group_no = '" & objdata.detail(i).item_group_no & "' " & _
                  "AND item_no = '" & objdata.detail(i).item_no & "' " & _
                  "ELSE " & _
                  "INSERT INTO tblPADetailBoss " & _
                  "(username, evaluate_year, round, item_group_no, item_no, " & _
                  "LoSBoss, comment, record_datetime, editor) " & _
                  "VALUES ('" & username & "', '" & evaluate_year & "', '" & round & "', " & _
                  "'" & objdata.detail(i).item_group_no & "', '" & objdata.detail(i).item_no & "', " & _
                  "'" & objdata.detail(i).LoSBoss & "', '" & Str2SQL(objdata.detail(i).comment) & "', " & _
                  "GETDATE(), '" & GetCurrentUser() & "')"
            
            pConnection.Execute(SQL)
        Next i

        pConnection.CommitTrans()

    Catch ex As Exception
        objJSONHeader.isError = True
        objJSONHeader.ErrMessage = ex.Message
        pConnection.RollbackTrans()
    Finally
        CloseDB(pConnection)
        ' ... response ...
    End Try
End Sub
```

### Step 2.3: Update JavaScript to Send FULL Data

**File**: `frmFormPA.js` - Method `$('#btnBossSave').click`

```javascript
$('#btnBossSave').click(function() {
    var username = $('#txtUserName').val();
    var evaluate_year = $('#lblBudgetYear').text();
    if (evaluate_year < 2500) { evaluate_year += 543; }
    var round = $('#txtRoundNo').val();
    
    var data = new tblPAHeader();
    data.username = username;
    data.evaluate_year = evaluate_year;
    data.round = round;  // ✓ FIX: Was assigning to data instead of detail
    data.suggest = $('#txtSuggest').val();
    
    var count = 0;
    for (item_group_no = 1; item_group_no <= 5; item_group_no++) {
        var new_id = 1;
        for (id = 1; id <= gRowNumArray[item_group_no-1]; id++) {
            if ($('#txtPAWeight_' + item_group_no + '_' + id).val() !== undefined) {
                var detail = new tblPADetail();
                detail.username = username;
                detail.evaluate_year = evaluate_year;  // ✓ FIX: Added to detail
                detail.round = round;                  // ✓ FIX: Added to detail
                detail.item_group_no = item_group_no;
                detail.item_no = new_id++;
                
                // ✓ FIX: Include ALL fields for reference/audit
                detail.detail = $('#txtPADetail_' + item_group_no + '_' + id).val();
                detail.detail2 = $('#txtPADetail2_' + item_group_no + '_' + id).val();
                detail.weight = $('#txtPAWeight_' + item_group_no + '_' + id).val();
                detail.KPI = $('#txtPAKPI_' + item_group_no + '_' + id).val();
                detail.STG = $('#txtPASTG_' + item_group_no + '_' + id).val();
                
                // Boss's contributions
                detail.LoSBoss = $('#selPALoSBoss_' + item_group_no + '_' + id).val();  
                detail.comment = $('#txtPAComment_' + item_group_no + '_' + id).val();
                
                data.detail[count++] = detail;
            }
        }
    }
    data.recordCount = count;

    var json = $.toJSON(data);
    $.ajax({
        url: 'frmPAService.aspx?q=BossSave&username=' + username + '&evaluate_year=' + evaluate_year + '&round=' + round + '&time=' + time(),
        type: 'POST',
        data: json,
        dataType: 'json',
        beforeSend: function() {
            $('#divBackground').show();
            $('#divWaiting').show();
        },
        complete: function() {
            $('#divBackground').hide();
            $('#divWaiting').hide();
        },
        success: function(json) {
            if (!json.isError) {
                alert('บันทึกสำเร็จ');
            } else {
                alert('บันทึกล้มเหลว: ' + json.ErrMessage);
            }
        }
    });
    
    return false;
});
```

---

## Testing Plan

### Test Case 1: User → Boss → User Edit
```
1. User opens frmFormPA
2. User enters: item1 (weight=20, KPI=100), comment=""
3. User clicks Save
   ✓ Verify: tblPADetail has the data
4. Boss opens same form
5. Boss fills: item1 LoSBoss=4, comment="Good"
6. Boss clicks Save
   ✓ Verify: tblPADetailBoss has LoSBoss, comment
   ✓ Verify: tblPADetail still has weight=20, KPI=100
7. User edits: changes weight to 25
8. User clicks Save
   ✓ Verify: tblPADetail updated to weight=25
   ✓ Verify: tblPADetailBoss unchanged

9. Load in both forms
   ✓ User sees their weight=25
   ✓ Boss sees their LoSBoss=4, comment="Good"
```

### Test Case 2: Deleted Rows Don't Corrupt Data
```
1. User creates 5 items
2. User saves
3. User deletes items 2 and 4
4. User saves
   ✓ Verify: Old item_no=2,4 marked as deleted
   ✓ Verify: Remaining items NOT renumbered in DB
5. Boss saves comments
   ✓ Verify: Comments saved to correct items (using old item_no)
```

---

## Files Requiring Changes Summary

| File | Method | Change | Priority |
|------|--------|--------|----------|
| frmPAService.aspx.vb | SavePAForm | Replace DELETE+INSERT | HIGH |
| frmPAService.aspx.vb | BossSave | Add error checking, use tblPADetailBoss | HIGH |
| frmPAService.aspx.vb | SavePAResult | Update to use tblPADetailResult | MEDIUM |
| frmPAService.aspx.vb | BossSaveResult | Update to use tblPADetailBossResult | MEDIUM |
| frmFormPA.js | DoSaveAndConfirm | Add validation | MEDIUM |
| frmFormPA.js | #btnBossSave | Send FULL data | HIGH |
| frmEvaluateFormPA.js | #btnBossSaveResult | Similar fixes | MEDIUM |
| Database | Schema | Add new tables, columns | HIGH |

---

## Expected Timeline

- **Today**: Phase 1 implementation (1-2 hours) - Quick workaround
- **Tomorrow**: Phase 2 testing (4-6 hours) - Core fix with DB changes
- **Next 2 days**: Phase 3 refactor (1-2 days) - Full concurrency control

Total: ~2-3 days for complete resolution
