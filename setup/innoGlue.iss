[Code]
type
TInnoCallbacksPointers = record
  ExpandConstant: longword;
	ExtractTemporaryFile: longword;
	ExtractTemporaryFiles: longword;
  CreatePage: longword;
end;

TExpandConstantProc = procedure(const S: WideString; out RetVal: WideString);
TExtractTemporaryFileProc = procedure(FileName: WideString); 
TExtractTemporaryFilesProc = function(Pattern: WideString): integer;
TRegisterCreatePageProc = function(const wndHandle: HWND; const AfterID: Integer; const ACaption, ADescription: WideString): integer;

function SetParent(hWndChild, hWndNewParent: HWND): HWND;
external 'SetParent@user32.dll stdcall';

// ----------------------------------------------------------------------------
// RegisterCallbacks
procedure RegisterCallbacks(pointers: TInnoCallbacksPointers);
external 'RegisterCallbacks@files:innoGlue.dll stdcall';

// ----------------------------------------------------------------------------
// Initialize
procedure SetupInitializeCallback();
external 'SetupInitializeCallback@files:innoGlue.dll stdcall';

// ----------------------------------------------------------------------------
// WizardInitialize
procedure WizardInitializeCallback();
external 'WizardInitializeCallback@files:innoGlue.dll stdcall';

// ----------------------------------------------------------------------------
// CurPageChangedEvent

procedure CurPageChangedCallback(curPageId: integer);
external 'CurPageChangedCallback@files:innoGlue.dll stdcall';

// ----------------------------------------------------------------------------
// ExpandConstant

function WrapExpandConstantProc(callback:TExpandConstantProc; paramcount:integer):longword;
  external 'wrapcallback@files:innocallback.dll stdcall';

procedure ExpandConstantWrapper(const S: WideString; out RetVal: WideString);
begin
	RetVal := ExpandConstant(S);
end;

// ----------------------------------------------------------------------------
// ExtractTemporaryFile

function WrapExtractTemporaryFileProc(callback:TExtractTemporaryFileProc; paramcount:integer):longword;
  external 'wrapcallback@files:innocallback.dll stdcall';

procedure ExtractTemporaryFileWrapper(const fileName: WideString);
begin
	ExtractTemporaryFile(fileName);
end;

// ----------------------------------------------------------------------------
// ExtractTemporaryFiles

function WrapExtractTemporaryFilesProc(callback:TExtractTemporaryFilesProc; paramcount:integer):longword;
  external 'wrapcallback@files:innocallback.dll stdcall';

function ExtractTemporaryFilesWrapper(const pattern: WideString) : integer;
begin
	Result := ExtractTemporaryFiles(pattern);
end;

// ----------------------------------------------------------------------------
// CreatePage

function WrapCreatePageProc(callback: TRegisterCreatePageProc; paramcount:integer): longword;
	external 'wrapcallback@files:innocallback.dll stdcall';
	
function CreatePageWrapper(const wndHandle :HWND; const AfterID: Integer; const ACaption, ADescription: WideString): integer;
var
	page: TWizardPage;
begin
	page := CreateCustomPage(AfterID, ACaption, ADescription);
	SetParent(wndHandle, page.Surface.Handle);
	Result := page.ID;
end;

// ----------------------------------------------------------------------------
procedure InitializeInnoGlue();
var
	pointers: TInnoCallbacksPointers;
begin
  pointers.ExpandConstant := WrapExpandConstantProc(@ExpandConstantWrapper, 2);	
	pointers.ExtractTemporaryFile := WrapExtractTemporaryFileProc(@ExtractTemporaryFileWrapper, 1);
	pointers.ExtractTemporaryFiles := WrapExtractTemporaryFilesProc(@ExtractTemporaryFilesWrapper, 1);
	pointers.CreatePage := WrapCreatePageProc(@CreatePageWrapper, 4);
	
  RegisterCallbacks(pointers);
	
	SetupInitializeCallback();
end;