unit global;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, StdCtrls;

Type IDCARD_ALL= record
  name : array[0..31] of char;
  sex : array[0..3] of char;
  nation : array[0..19] of char;
  birthday : array[0..11] of char;
  address : array[0..71] of char;
  cardId : array[0..19] of char;
  validStart : array[0..11] of char;
  validEnd : array[0..11] of char;
  sexCode : array[0..3] of char;
  nationCode : array[0..3] of char;
  appendMsg : array[0..71] of char;
end;

type pointerID=^IDCARD_ALL;

type
  TOpenCardReader = function(lPort : string;ulFlag : string): integer;stdcall;
  //TCalibrateScanner = procedure();stdcall;
 // TGetButtonDownType = function(): integer;stdcall;
  TGetPersonMsgA = function(pInfo : pointerID; pszImageFile : string) : integer;stdcall;
var
  SHandle : THandle;
  POpenCardReader : TOpenCardReader;
  PGetPersonMsgA : TGetPersonMsgA;
 // PCalibrateScanner : TCalibrateScanner;
//  PGetButtonDownType : TGetButtonDownType;
 // PRecogNewIdcardALL : TRecogNewIdcardALL;
  function LoadCardDll() : Boolean;
 // procedure FreeScanDll();
implementation
function LoadCardDll() : Boolean;
begin
  Result := False;
  SHandle := LoadLibrary('cardapi2.dll');
  if SHandle<>0 then
  begin
    POpenCardReader := GetProcAddress(SHandle,'OpenCardReader');
    PGetPersonMsgA := GetProcAddress(SHandle,'GetPersonMsgA');
    //PCalibrateScanner := GetProcAddress(SHandle,'CalibrateScanner');
    //PGetButtonDownType := GetProcAddress(SHandle,'GetButtonDownType');
    //PRecogNewIdcardALL := GetProcAddress(SHandle,'RecogNewIdcardALL');
  end
  else
    ShowMessage('Î´ÕÒµ½¶¯Ì¬Á´½Ó¿âcardapi2.dll');
  Result := True;
end;
//procedure FreeScanDll();
//begin
//  FreeLibrary(SHandle);
//end;
end.

