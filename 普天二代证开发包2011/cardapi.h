#pragma once

#define PORTMANU 0x01
#define REPEATREAD 0x02
#define NEWADDRESS 0x04

typedef struct _personinfoa
{
	char name[32];
	char sex[4];
	char nation[20];
	char birthday[12];
	char address[72];
	char cardId[20];
	char police[32];
	char validStart[12];
	char validEnd[12];
	char sexCode[4];
	char nationCode[4];
	char appendMsg[72];
}PERSONINFOA,*PPERSONINFOA;

typedef struct _personinfow
{
	wchar_t name[16];
	wchar_t sex[2];
	wchar_t nation[10];
	wchar_t birthday[10];
	wchar_t address[36];
	wchar_t cardId[20];
	wchar_t police[16];
	wchar_t validStart[10];
	wchar_t validEnd[10];
	wchar_t sexCode[2];
	wchar_t nationCode[4];
	wchar_t appendMsg[36];
}PERSONINFOW,*PPERSONINFOW;

long __stdcall OpenCardReader(long lPort, ULONG ulFlag, ULONG ulBaudRate);
long __stdcall GetPersonMsgW(PPERSONINFOW pInfo, const wchar_t* pszImageFile);
long __stdcall GetPersonMsgA(PPERSONINFOA pInfo, const char* pszImageFile);
long __stdcall ResetCardReader();
long __stdcall CloseCardReader();
long __stdcall GetCardReaderStatus(long lPort, ULONG ulBaudRate);
void __stdcall GetErrorTextW(wchar_t* pszBuffer, DWORD dwBufLen);
void __stdcall GetErrorTextA(char* pszBuffer, DWORD dwBufLen);

#ifdef _UNICODE
#define PERSONINFO PERSONINFOW
#define PPERSONINFO PPERSONINFOW
#define GetPersonMsg GetPersonMsgW
#define GetErrorText GetErrorTextW
#else
#define PERSONINFO PERSONINFOA
#define PPERSONINFO PPERSONINFOA
#define GetPersonMsg GetPersonMsgA
#define GetErrorText GetErrorTextA
#endif

typedef long (__stdcall* defOpenCardReader)(long, ULONG, ULONG);
typedef long (__stdcall* defGetPersonMsgW)(PPERSONINFOW, const wchar_t*);
typedef long (__stdcall* defGetPersonMsgA)(PPERSONINFOA, const char*);
typedef long (__stdcall* defResetCardReader)();
typedef long (__stdcall* defCloseCardReader)();
typedef long (__stdcall* defGetCardReaderStatus)(long, ULONG);
typedef void (__stdcall* defGetErrorTextW)(wchar_t*, DWORD);
typedef void (__stdcall* defGetErrorTextA)(char*, DWORD);
