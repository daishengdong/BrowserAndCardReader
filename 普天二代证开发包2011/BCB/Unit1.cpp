//---------------------------------------------------------------------------

#include <vcl.h>
#pragma hdrstop

#include "Unit1.h"
#include "cardapi.h"
//---------------------------------------------------------------------------
#pragma package(smart_init)
#pragma resource "*.dfm"
TForm1 *Form1;
//---------------------------------------------------------------------------
__fastcall TForm1::TForm1(TComponent* Owner)
        : TForm(Owner)
{
}
//---------------------------------------------------------------------------

void __fastcall TForm1::BtnOpenClick(TObject *Sender)
{
        TCHAR szErrorText[32];

        OpenCardReader(0,2,115200);
        GetErrorText(szErrorText,32);
        EditResult->Text=szErrorText;
}
//---------------------------------------------------------------------------
void __fastcall TForm1::BtnReadClick(TObject *Sender)
{
        long result;
        PERSONINFO person;
        TCHAR szErrorText[32];

        result=GetPersonMsg(&person,TEXT("ssss.bmp"));
        if(0==result)
        {
                Edit1->Text=person.name;
                Edit2->Text=person.sex;
                Edit3->Text=person.nation;
                Edit4->Text=person.birthday;
                Edit5->Text=person.address;
                Edit6->Text=person.cardId;
                Edit7->Text=person.police;
                Edit8->Text=person.validStart;
                Edit9->Text=person.validEnd;
                Image1->Picture->LoadFromFile(TEXT("ssss.bmp"));
        }
        else
        {
                Edit1->Text=L"";
                Edit2->Text=L"";
                Edit3->Text=L"";
                Edit4->Text=L"";
                Edit5->Text=L"";
                Edit6->Text=L"";
                Edit7->Text=L"";
                Edit8->Text=L"";
                Edit9->Text=L"";
                Image1->Picture->Assign(NULL);
        }
        GetErrorText(szErrorText,32);
        EditResult->Text=szErrorText;
}
//---------------------------------------------------------------------------
void __fastcall TForm1::BtnCloseClick(TObject *Sender)
{
        TCHAR szErrorText[32];

        CloseCardReader();
        GetErrorText(szErrorText,32);
        EditResult->Text=szErrorText;
}
//---------------------------------------------------------------------------
