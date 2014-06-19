// Test4Dlg.h : ͷ�ļ�
//

#pragma once
#include "cardapi.h"

// CTest4Dlg �Ի���
class CTest4Dlg : public CDialog
{
// ����
public:
	CTest4Dlg(CWnd* pParent = NULL);	// ��׼���캯��

// �Ի�������
	enum { IDD = IDD_TEST4_DIALOG };

	protected:
	virtual void DoDataExchange(CDataExchange* pDX);	// DDX/DDV ֧��


// ʵ��
protected:
	HICON m_hIcon;
	HBITMAP m_hPhoto;
	PERSONINFO m_person;
	TCHAR m_szBirthday[16];
	TCHAR m_szValidDate[24];
	// ���ɵ���Ϣӳ�亯��
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
