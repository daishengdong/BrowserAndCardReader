unit Unit1;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, StdCtrls, ExtCtrls;

type
  IDCARD_ALL = record
{    name : array[0..31] of char;
  sex : array[0..3] of char;
  nation : array[0..19] of char;
  birthday : array[0..11] of char;
  address : array[0..71] of char;
  cardId : array[0..19] of char;
  police: array[0..31] of char;
  validStart : array[0..11] of char;
  validEnd : array[0..11] of char;
  sexCode : array[0..3] of char;
  nationCode : array[0..3] of char;
  appendMsg : array[0..71] of char; }
  name:array[0..15]of WideChar;
  sex:array[0..1] of WideChar;
  nation:array[0..9] of WideChar;
  birthday:array[0..9] of WideChar;
  address:array[0..35] of WideChar;
  cardId: array[0..19] of WideChar;
  police:array[0..15] of WideChar;
  validStart:array[0..9] of WideChar;
  validEnd:array[0..9] of WideChar;
  sexCode:array[0..1] of WideChar;
  nationCode:array[0..3] of WideChar;
  appendMsg:array[0..35] of WideChar;
  end;
  TForm1 = class(TForm)
    Button2: TButton;
    Button1: TButton;
    Edit1: TEdit;
    Edit2: TEdit;
    Edit3: TEdit;
    Edit4: TEdit;
    Edit5: TEdit;
    Edit6: TEdit;
    Edit7: TEdit;
    Edit8: TEdit;
    Edit9: TEdit;
    Button3: TButton;
    Label1: TLabel;
    EditResult: TEdit;
    Label2: TLabel;
    Label3: TLabel;
    Label4: TLabel;
    Label5: TLabel;
    Label6: TLabel;
    Label7: TLabel;
    Label8: TLabel;
    Label9: TLabel;
    Image1: TImage;
    procedure Button1Click(Sender: TObject);
    procedure Button2Click(Sender: TObject);
    procedure Button3Click(Sender: TObject);

  private
    { Private declarations }
  public
    { Public declarations }
  end;

var
  Form1: TForm1;
  function OpenCardReader(lPort:LongInt; ulFlag:LongWord; ulBaudRate:LongWord): Longint; stdcall; external'cardapi3.dll';
  {function GetPersonMsgA(var card:IDCARD_ALL; pszImageFile:PChar): Longint; stdcall; external'cardapi3.dll';  }
  function GetPersonMsgW(var card:IDCARD_ALL; pszImageFile:PWideChar): Longint; stdcall; external 'cardapi3.dll';
  function CloseCardReader(): Longint;stdcall; external'cardapi3.dll';
  Procedure GetErrorTextW(pszBuffer:PWideChar; dwBufLen:LongWord); stdcall; external'cardapi3.dll';

implementation

{$R *.dfm}

procedure TForm1.Button1Click(Sender: TObject);
var
  errorText:array[0..31] of WideChar;
begin
  OpenCardReader(0,2,115200);
  GetErrorTextW(errorText,32);
  EditResult.Text:=errorText;
end;

procedure TForm1.Button2Click(Sender: TObject);
var
  rel: Integer;
  card: IDCARD_ALL;
  errorText:array[0..31] of WideChar;
begin
    rel := GetPersonMsgW(card,'ssss.bmp');
    if rel=0 then
    begin
      edit1.Text := card.name;
      edit2.Text := card.sex;
      edit3.Text := card.nation;
      edit4.Text := card.birthday;
      edit5.Text := card.address;
      edit6.Text := card.cardId;
      edit7.Text := card.police;
      edit8.Text := card.validStart;
      edit9.Text := card.validEnd;
      Image1.Picture.LoadFromFile('ssss.bmp');
    end
    else
    begin
      edit1.Text:='';
      edit2.Text:='';
      edit3.Text:='';
      edit4.Text:='';
      edit5.Text:='';
      edit6.Text:='';
      edit7.Text:='';
      edit8.Text:='';
      edit9.Text:='';
      Image1.Picture.Assign(nil);
    end;
  GetErrorTextW(errorText,32);
  EditResult.Text:=errorText;
end;

procedure TForm1.Button3Click(Sender: TObject);
var
  errorText:array[0..31] of WideChar;
begin
  CloseCardReader();
  GetErrorTextW(errorText,32);
  EditResult.Text:=errorText;
end;



end.
 