// Test4Dlg.cpp : 实现文件
//

#include "stdafx.h"
#include "Test4.h"
#include "Test4Dlg.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif


// CTest4Dlg 对话框




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


// CTest4Dlg 消息处理程序

BOOL CTest4Dlg::OnInitDialog()
{
	CDialog::OnInitDialog();

	// 设置此对话框的图标。当应用程序主窗口不是对话框时，框架将自动
	//  执行此操作
	SetIcon(m_hIcon, TRUE);			// 设置大图标
	SetIcon(m_hIcon, FALSE);		// 设置小图标

	// TODO: 在此添加额外的初始化代码

	return TRUE;  // 除非将焦点设置到控件，否则返回 TRUE
}

// 如果向对话框添加最小化按钮，则需要下面的代码
//  来绘制该图标。对于使用文档/视图模型的 MFC 应用程序，
//  这将由框架自动完成。
__inline void CTest4Dlg::DrawString(CDC &dc, LPCTSTR lpString, LPRECT lpRect, UINT uFormat)
{
	dc.DrawText(lpString,_tcslen(lpString),lpRect,uFormat);
}

void CTest4Dlg::OnPaint()
{
	if (IsIconic())
	{
		CPaintDC dc(this); // 用于绘制的设备上下文

		SendMessage(WM_ICONERASEBKGND, reinterpret_cast<WPARAM>(dc.GetSafeHdc()), 0);

		// 使图标在工作矩形中居中
		int cxIcon = GetSystemMetrics(SM_CXICON);
		int cyIcon = GetSystemMetrics(SM_CYICON);
		CRect rect;
		GetClientRect(&rect);
		int x = (rect.Width() - cxIcon + 1) / 2;
		int y = (rect.Height() - cyIcon + 1) / 2;

		// 绘制图标
		dc.DrawIcon(x, y, m_hIcon);
	}
	else
	{
		HDC hdcMem;
		HBITMAP hBmpOld;
		HFONT hFont,hFontOld;
		LOGFONT font;
		RECT rect;

		CPaintDC dc(this); // 用于绘制的设备上下文
		dc.SetBkMode(TRANSPARENT);

		memset(&font,0,sizeof(LOGFONT));
		font.lfHeight=14;
		font.lfWeight=FW_NORMAL;
		font.lfCharSet=DEFAULT_CHARSET;
		_tcscpy_s(font.lfFaceName,32,_T("宋体"));
		hFont=CreateFontIndirect(&font);
		hFontOld=(HFONT)SelectObject(dc.m_hDC,hFont);
		dc.SetTextColor(RGB(192,128,128));
		SetRect(&rect, 10, 10, 100, 30);
		DrawString(dc, _T("姓名"), &rect, DT_LEFT|DT_TOP);
		SetRect(&rect, 10, 30, 100, 50);
		DrawString(dc, _T("性别"), &rect, DT_LEFT|DT_TOP);
		SetRect(&rect, 10, 50, 100, 70);
		DrawString(dc, _T("民族"), &rect, DT_LEFT|DT_TOP);
		SetRect(&rect, 10, 70, 100, 90);
		DrawString(dc, _T("出生日期"), &rect, DT_LEFT|DT_TOP);
		SetRect(&rect, 10, 90, 100, 110);
		DrawString(dc, _T("地址"), &rect, DT_LEFT|DT_TOP);
		SetRect(&rect, 10, 130, 100, 150);
		DrawString(dc, _T("身份证号码"), &rect, DT_LEFT|DT_TOP);
		SetRect(&rect, 10, 150, 100, 170);
		DrawString(dc, _T("签发机关"), &rect, DT_LEFT|DT_TOP);
		SetRect(&rect, 10, 170, 100, 190);
		DrawString(dc, _T("有效期限"), &rect, DT_LEFT|DT_TOP);

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

//当用户拖动最小化窗口时系统调用此函数取得光标显示。
//
HCURSOR CTest4Dlg::OnQueryDragIcon()
{
	return static_cast<HCURSOR>(m_hIcon);
}

void CTest4Dlg::OnBnClickedBtnopen()
{
	// TODO: 在此添加控件通知处理程序代码
	long result;
	TCHAR szErrorText[32];
	/*参数1为端口号。1表示串口1，2表示串口2，依次类推。1001表示USB。0表示自动选择。
	  参数2为标志位。0x02表示启用重复读卡。0x04表示读卡后接着读取新地址。
	  各个数值可以用“按位或”运算符组合起来。
	  参数3为波特率。使用串口阅读器的程序应正确设置此参数。出厂机器的波特率一般为115200。
	*/
	result=OpenCardReader(0,2,115200);
	SetDlgItemInt(IDC_EDRTN,result,TRUE);
	GetErrorText(szErrorText,_countof(szErrorText));
	SetDlgItemText(IDC_EDMSG,szErrorText);
}

void CTest4Dlg::OnBnClickedBtnread()
{
	// TODO: 在此添加控件通知处理程序代码
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
	//当程序打开设备后，可以多次调用读取信息函数。
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
	// TODO: 在此添加控件通知处理程序代码
	long result;
	TCHAR szErrorText[32];
	//读完卡后，必须关闭设备。
	result=CloseCardReader();
	SetDlgItemInt(IDC_EDRTN,result,TRUE);
	GetErrorText(szErrorText,_countof(szErrorText));
	SetDlgItemText(IDC_EDMSG,szErrorText);
}

void CTest4Dlg::OnDestroy()
{
	CDialog::OnDestroy();

	// TODO: 在此处添加消息处理程序代码
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
		_stprintf_s(dst,nDstLen,_T("%s年%s月%s日"),year,month,day);
	}
	else
	{
		if(_tcscmp(src,_T("长期"))==0)
			_tcscpy_s(dst,nDstLen,_T("长期"));
		else
			_stprintf_s(dst,nDstLen,_T("%s.%s.%s"),year,month,day);
	}
}
