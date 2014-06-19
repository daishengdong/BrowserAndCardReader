import java.io.*;
import com.sun.jna.*;
import com.sun.jna.win32.*;

public class Card2{
	public static class PERSONINFOW extends Structure{
		public PERSONINFOW(){
			setAlignType(ALIGN_MSVC);
		}
		public char[]name		=new char[16];
		public char[]sex		=new char[2];
		public char[]nation		=new char[10];
		public char[]birthday	=new char[10];
		public char[]address	=new char[36];
		public char[]cardId		=new char[20];
		public char[]police		=new char[16];
		public char[]validStart	=new char[10];
		public char[]validEnd	=new char[10];
		public char[]sexCode	=new char[2];
		public char[]nationCode	=new char[4];
		public char[]appendMsg	=new char[36];
	}

	public interface CardApi extends StdCallLibrary{
		CardApi INSTANCE =(CardApi)Native.loadLibrary("cardapi3.dll",CardApi.class);
		public int OpenCardReader(int lPort, int ulFlag, int ulBaudRate);
		public int GetPersonMsgW(PERSONINFOW pInfo, WString pszImageFile);
		public int CloseCardReader();
		public void GetErrorTextW(char[]pszBuffer, int dwBufLen);
	}

	public static String CStrToString(char[]str){
		int i=0;
		
		while(i<str.length && str[i]!=0){
			i++;
		}
		return new String(str,0,i);
	}
	
	public static void main(String[]args) throws IOException{
		int ch;
		int result;
		PERSONINFOW person=new PERSONINFOW();
		String str=System.getProperty("java.io.tmpdir")+"image.bmp";
		WString imageFile=new WString(str);
		char[]errorText=new char[32];
		
		System.out.println("按Enter键打开端口");
		ch=System.in.read();
		System.in.read();

		result=CardApi.INSTANCE.OpenCardReader(0,2,115200);
		if(result!=0)
		{
			System.out.println("端口打开失败："+Integer.toString(result));
			return;
		}
		do
		{
			System.out.println("按'q'键退出，按Enter键读卡");
			ch=System.in.read();
			System.in.read();
			if(ch=='q')
				break;
			result=CardApi.INSTANCE.GetPersonMsgW(person,imageFile);
			System.out.println("返回值："+Integer.toString(result));
			if(result==0)
			{
				System.out.println(CStrToString(person.name));
				System.out.println(CStrToString(person.sex));
				System.out.println(CStrToString(person.nation));
				System.out.println(CStrToString(person.birthday));
				System.out.println(CStrToString(person.address));
				System.out.println(CStrToString(person.cardId));
				System.out.println(CStrToString(person.police));
				System.out.println(CStrToString(person.validStart));
				System.out.println(CStrToString(person.validEnd));
			}
			else
			{
				CardApi.INSTANCE.GetErrorTextW(errorText,errorText.length);
				System.out.println(CStrToString(errorText));
			}
		}while(true);
		CardApi.INSTANCE.CloseCardReader();
	}
}
