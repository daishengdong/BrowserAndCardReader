// Test4Dlg.cpp : ʵ���ļ�
//

#include "stdafx.h"
#include "Test4.h"
#include "Test4Dlg.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif


// CTest4Dlg �Ի���




CTest4Dlg::CTest4Dlg(CWnd* pParent /*=NULL*/)
	: CDialog(CTest4Dlg::IDD, pParent)
	,m_hPhoto(NULL)
{
	m_hIcon = AfxGetApp()->LoadIcon(IDR_MAINFRAME);
	memset(&m_person,0,sizeof(PERSONINFO));
	m_szBirthday[0]=0;
	m_szValidDate[0]=0;
}

void CTest4Dlg::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
}

BEGIN_MESSAGE_MAP(CTest4Dlg, CDialog)
	ON_WM_PAINT()
	ON_WM_QUERYDRAGICON()
	//}}AFX_MSG_MAP
	ON_BN_CLICKED(IDC_BTNOPEN, &CTest4Dlg::OnBnClickedBtnopen)
	ON_BN_CLICKED(IDC_BTNREAD, &CTest4Dlg::OnBnClickedBtnread)
	ON_BN_CLICKED(IDC_BTNCLOSE, &CTest4Dlg::OnBnClickedBtnclose)
	ON_WM_DESTROY()
END_MESSAGE_MAP()


// CTest4Dlg ��Ϣ�������

BOOL CTest4Dlg::OnInitDialog()
{
	CDialog::OnInitDialog();

	// ���ô˶Ի����ͼ�ꡣ��Ӧ�ó��������ڲ��ǶԻ���ʱ����ܽ��Զ�
	//  ִ�д˲���
	SetIcon(m_hIcon, TRUE);			// ���ô�ͼ��
	SetIcon(m_hIcon, FALSE);		// ����Сͼ��

	// TODO: �ڴ���Ӷ���ĳ�ʼ������

	return TRUE;  // ���ǽ��������õ��ؼ������򷵻� TRUE
}

// �����Ի��������С����ť������Ҫ����Ĵ���
//  �����Ƹ�ͼ�ꡣ����ʹ���ĵ�/��ͼģ�͵� MFC Ӧ�ó���
//  �⽫�ɿ���Զ���ɡ�
__inline void CTest4Dlg::DrawString(CDC &dc, LPCTSTR lpString, LPRECT lpRect, UINT uFormat)
{
	dc.DrawText(lpString,_tcslen(lpString),lpRect,uFormat);
}

void CTest4Dlg::OnPaint()
{
	if (IsIconic())
	{
		CPaintDC dc(this); // ���ڻ��Ƶ��豸������

		SendMessage(WM_ICONERASEBKGND, reinterpret_cast<WPARAM>(dc.GetSafeHdc()), 0);

		// ʹͼ���ڹ��������о���
		int cxIcon = GetSystemMetrics(SM_CXICON);
		int cyIcon = GetSystemMetrics(SM_CYICON);
		CRect rect;
		GetClientRect(&rect);
		int x = (rect.Width() - cxIcon + 1) / 2;
		int y = (rect.Height() - cyIcon + 1) / 2;

		// ����ͼ��
		dc.DrawIcon(x, y, m_hIcon);
	}
	else
	{
		HDC hdcMem;
		HBITMAP hBmpOld;
		HFONT hFont,hFontOld;
		LOGFONT font;
		RECT rect;

		CPaintDC dc(this); // ���ڻ��Ƶ��豸������
		dc.SetBkMode(TRANSPARENT);

		memset(&font,0,sizeof(LOGFONT));
		font.lfHeight=14;
		font.lfWeight=FW_NORMAL;
		font.lfCharSet=DEFAULT_CHARSET;
		_tcscpy_s(font.lfFaceName,32,_T("����"));
		hFont=CreateFontIndirect(&font);
		hFontOld=(HFONT)SelectObject(dc.m_hDC,hFont);
		dc.SetTextColor(RGB(192,128,128));
		SetRect(&rect, 10, 10, 100, 30);
		DrawString(dc, _T("����"), &rect, DT_LEFT|DT_TOP);
		SetRect(&rect, 10, 30, 100, 50);
		DrawString(dc, _T("�Ա�"), &rect, DT_LEFT|DT_TOP);
		SetRect(&rect, 10, 50, 100, 70);
		DrawString(dc, _T("����"), &rect, DT_LEFT|DT_TOP);
		SetRect(&rect, 10, 70, 100, 90);
		DrawString(dc, _T("��������"), &rect, DT_LEFT|DT_TOP);
		SetRect(&rect, 10, 90, 100, 110);
		DrawString(dc, _T("��ַ"), &rect, DT_LEFT|DT_TOP);
		SetRect(&rect, 10, 130, 100, 150);
		DrawString(dc, _T("���֤����"), &rect, DT_LEFT|DT_TOP);
		SetRect(&rect, 10, 150, 100, 170);
		DrawString(dc, _T("ǩ������"), &rect, DT_LEFT|DT_TOP);
		SetRect(&rect, 10, 170, 100, 190);
		DrawString(dc, _T("��Ч����"), &rect, DT_LEFT|DT_TOP);

		if (m_person.name[0] != 0)
		{
			dc.SetTextColor(RGB(0,0,0));
			SetRect(&rect, 100, 10, 400, 30);
			DrawString(dc, m_person.name, &rect, DT_LEFT|DT_TOP);
			SetRect(&rect, 100, 30, 200, 50);
			DrawString(dc, m_person.sex, &rect, DT_LEFT|DT_TOP);
			SetRect(&rect, 100, 50, 250, 70);
			DrawString(dc, m_person.nation, &rect, DT_LEFT|DT_TOP);
			SetRect(&rect, 100, 70, 400, 90);
			DrawString(dc, m_szBirthday, &rect, DT_LEFT|DT_TOP);
			SetRect(&rect, 100, 90, 400, 130);
			DrawString(dc, m_person.address, &rect, DT_LEFT|DT_TOP|DT_WORDBREAK);
			SetRect(&rect, 100, 130, 400, 150);
			DrawString(dc, m_person.cardId, &rect, DT_LEFT|DT_TOP);
			SetRect(&rect, 100, 150, 400, 170);
			DrawString(dc, m_person.police, &rect, DT_LEFT|DT_TOP);
			SetRect(&rect, 100, 170, 400, 190);
			DrawString(dc, m_szValidDate, &rect, DT_LEFT|DT_TOP);
		}
		SelectObject(dc.m_hDC,hFontOld);
		DeleteObject(hFont);

		if(m_hPhoto)
		{
			hdcMem=CreateCompatibleDC(dc.m_hDC);
			hBmpOld=(HBITMAP)SelectObject(hdcMem,m_hPhoto);
			BitBlt(dc.m_hDC,400,10,102,126,hdcMem,0,0,SRCCOPY);
			SelectObject(hdcMem,hBmpOld);
			DeleteDC(hdcMem);
		}
	}
}

//���û��϶���С������ʱϵͳ���ô˺���ȡ�ù����ʾ��
//
HCURSOR CTest4Dlg::OnQueryDragIcon()
{
	return static_cast<HCURSOR>(m_hIcon);
}

void CTest4Dlg::OnBnClickedBtnopen()
{
	// TODO: �ڴ���ӿؼ�֪ͨ����������
	long result;
	TCHAR szErrorText[32];
	/*����1Ϊ�˿ںš�1��ʾ����1��2��ʾ����2���������ơ�1001��ʾUSB��0��ʾ�Զ�ѡ��
	  ����2Ϊ��־λ��0x02��ʾ�����ظ�������0x04��ʾ��������Ŷ�ȡ�µ�ַ��
	  ������ֵ�����á���λ����������������
	  ����3Ϊ�����ʡ�ʹ�ô����Ķ����ĳ���Ӧ��ȷ���ô˲��������������Ĳ�����һ��Ϊ115200��
	*/
	result=OpenCardReader(0,2,115200);
	SetDlgItemInt(IDC_EDRTN,result,TRUE);
	GetErrorText(szErrorText,_countof(szErrorText));
	SetDlgItemText(IDC_EDMSG,szErrorText);
}

void CTest4Dlg::OnBnClickedBtnread()
{
	// TODO: �ڴ���ӿؼ�֪ͨ����������
	long result;
	TCHAR szFile[MAX_PATH];
	TCHAR szErrorText[32];

	if(m_hPhoto)
	{
		DeleteObject(m_hPhoto);
		m_hPhoto=NULL;
	}
	GetTempPath(MAX_PATH,szFile);
	_tcscat_s(szFile,MAX_PATH,_T("image.bmp"));
	//��������豸�󣬿��Զ�ε��ö�ȡ��Ϣ������
	result=GetPersonMsg(&m_person,szFile);
	SetDlgItemInt(IDC_EDRTN,result,TRUE);
	GetErrorText(szErrorText,_countof(szErrorText));
	SetDlgItemText(IDC_EDMSG,szErrorText);

	if(0==result)
	{
		ConvertDate(m_szBirthday,_countof(m_szBirthday),m_person.birthday,1);
		ConvertDate(m_szValidDate,12,m_person.validStart,2);
		m_szValidDate[10]='-';
		ConvertDate(m_szValidDate+11,12,m_person.validEnd,2);
		m_hPhoto=(HBITMAP)LoadImage(NULL,szFile,IMAGE_BITMAP,0,0,LR_LOADFROMFILE);
	}
	else
	{
		m_szBirthday[0]=0;
		m_szValidDate[0]=0;
	}
	InvalidateRect(NULL,TRUE);
}

void CTest4Dlg::OnBnClickedBtnclose()
{
	// TODO: �ڴ���ӿؼ�֪ͨ����������
	long result;
	TCHAR szErrorText[32];
	//���꿨�󣬱���ر��豸��
	result=CloseCardReader();
	SetDlgItemInt(IDC_EDRTN,result,TRUE);
	GetErrorText(szErrorText,_countof(szErrorText));
	SetDlgItemText(IDC_EDMSG,szErrorText);
}

void CTest4Dlg::OnDestroy()
{
	CDialog::OnDestroy();

	// TODO: �ڴ˴������Ϣ����������
	if(m_hPhoto)
	{
		DeleteObject(m_hPhoto);
		m_hPhoto=NULL;
	}
}

void CTest4Dlg::ConvertDate(LPTSTR dst, int nDstLen, LPCTSTR src, int mode)
{
	TCHAR year[8];
	TCHAR month[4];
	TCHAR day[4];

	_tcsncpy_s(year,8,src,4);
	_tcsncpy_s(month,4,src+4,2);
	_tcsncpy_s(day,4,src+6,2);

	if(1==mode)
	{
		_stprintf_s(dst,nDstLen,_T("%s��%s��%s��"),year,month,day);
	}
	else
	{
		if(_tcscmp(src,_T("����"))==0)
			_tcscpy_s(dst,nDstLen,_T("����"));
		else
			_stprintf_s(dst,nDstLen,_T("%s.%s.%s"),year,month,day);
	}
}
