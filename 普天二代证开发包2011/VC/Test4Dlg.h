// Test4Dlg.h : 头文件
//

#pragma once
#include "cardapi.h"

// CTest4Dlg 对话框
class CTest4Dlg : public CDialog
{
// 构造
public:
	CTest4Dlg(CWnd* pParent = NULL);	// 标准构造函数

// 对话框数据
	enum { IDD = IDD_TEST4_DIALOG };

	protected:
	virtual void DoDataExchange(CDataExchange* pDX);	// DDX/DDV 支持


// 实现
protected:
	HICON m_hIcon;
	HBITMAP m_hPhoto;
	PERSONINFO m_person;
	TCHAR m_szBirthday[16];
	TCHAR m_szValidDate[24];
	// 生成的消息映射函数
	virtual BOOL OnInitDialog();
	afx_msg void OnPaint();
	afx_msg HCURSOR OnQueryDragIcon();
	DECLARE_MESSAGE_MAP()
public:
	__inline void DrawString(CDC &dc, LPCTSTR lpString, LPRECT lpRect, UINT uFormat);
	void ConvertDate(LPTSTR dst, int nDstLen, LPCTSTR src, int mode);
	afx_msg void OnBnClickedBtnopen();
	afx_msg void OnBnClickedBtnread();
	afx_msg void OnBnClickedBtnclose();
	afx_msg void OnDestroy();
};
