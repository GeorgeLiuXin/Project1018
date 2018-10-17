using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Reflection;
using UnityEditor;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Text.RegularExpressions;
using Microsoft.CSharp;
using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential,CharSet = CharSet.Auto)]
public class OpenFileName
{
	public int structSize = 0;
	public IntPtr dlgOwner = IntPtr.Zero;
	public IntPtr instance = IntPtr.Zero;
	public String filter = null;
	public String customFilter = null;
	public int 	  maxCustFilter = 0;
	public int filterIndex = 0;
	public String file = null;
	public int maxFile = 0;
	public String fileTitle = null;
	public int maxFileTitle = 0;
	public String initialDir = null;
	public String title = null;
	public int flags = 0;
	public short fileOffset = 0;
	public short fileExtension = 0;
	public String defExt = null;
	public IntPtr custData = IntPtr.Zero;
	public IntPtr hook = IntPtr.Zero;
	public String tempName = null;
	public IntPtr reserverPtr = IntPtr.Zero;
	public int reserverInt = 0;
	public int flagsEx =0;
}

public static class AutoCSharpTools
{

	public class LocalDialog
	{
		[DllImport("Comdlg32.dll",SetLastError = true, ThrowOnUnmappableChar = true,CharSet = CharSet.Auto)]
		public static extern bool GetOpenFileName ([In,Out] OpenFileName ofn);
		public static bool GetOFN([In,Out] OpenFileName ofn)
		{
			return GetOpenFileName(ofn);
		}
	}

	public class CSharpCalssBuilder
	{
		public CodeCompileUnit unit 
		{
			private set;
			get;
		}

        public CodeCompileUnit unit_manager
        {
            private set;
            get;
        }

        public CodeCompileUnit unit_manager_base
        {
            private set;
            get;
        }

        public CodeNamespace customNameSpace 
		{
			set;
			get;
		}
        public CodeNamespace customNameSpace_manager
        {
            set;
            get;
        }

        public CodeTypeDeclaration customerclass
		{
			set;
			get;
		}

		public CodeTypeDeclaration tableClass
		{
			set;
			get;
		}

        public CodeTypeDeclaration tableClass_manager
        {
            set;
            get;
        }

        public CodeTypeDeclaration tableClass_manager_base
        {
            private set;
            get;
        }

        public CodeMemberField configTable 
		{
			set;
			get;
		}

		public CodeMemberMethod setMethod 
		{
			set;
			get;
		}



        public CodeMemberMethod setMethod_manager
        {
            set;
            get;
        }

        public CodeMemberMethod staticMethod_manager
        {
            get;
            set;
        }

        public CodeMemberMethod setMethod_privateSet
        {
            get;
            set;
        }

        public List<CodeMemberField>	m_filed = new List<CodeMemberField> ();
		public List<string> 		   m_typeSave = new List<string>();

        public void BuildManagerClass()
        {
            unit_manager = null;
            unit_manager = new CodeCompileUnit();

            customNameSpace_manager = new CodeNamespace("XWorld.DataConfig");
            customNameSpace_manager.Imports.Add(new CodeNamespaceImport("UnityEngine"));
            customNameSpace_manager.Imports.Add(new CodeNamespaceImport("System.Collections.Generic"));
            customNameSpace_manager.Imports.Add(new CodeNamespaceImport("System"));
            unit_manager.Namespaces.Add(customNameSpace_manager);

            tableClass_manager = new CodeTypeDeclaration("ClientConfigManager");
            tableClass_manager.IsClass = true;
            tableClass_manager.IsPartial = true;
            tableClass_manager.TypeAttributes = TypeAttributes.Public;
            tableClass_manager.BaseTypes.Add(new CodeTypeReference("MonoBehaviour"));
            customNameSpace_manager.Types.Add(tableClass_manager);

            /*setMethod_manager.Name = "LoadAllDynamicConfigs";
            setMethod_manager.Statements.Add(new CodeTypeReferenceExpression("string context = string.Empty"));

            staticMethod_manager = new CodeMemberMethod();
            staticMethod_manager.Attributes = MemberAttributes.Public | MemberAttributes.Final | MemberAttributes.Static;
            tableClass_manager.Members.Add(staticMethod_manager);

            staticMethod_manager.Name = "LoadAllStaticConfig";
            staticMethod_manager.Statements.Add(new CodeTypeReferenceExpression("string context"));*/

            /*setMethod_privateSet.Name = "InsertKeyValue";
            CodeParameterDeclarationExpression param1 = new CodeParameterDeclarationExpression("string", "strClassName");
            setMethod_privateSet.Parameters.Add(param1);
            CodeParameterDeclarationExpression param2 = new CodeParameterDeclarationExpression("XWorld.DataConfig.TableLoaderBase", "classType");
            setMethod_privateSet.Parameters.Add(param2);

            CodeMethodReferenceExpression codeRefExp = new CodeMethodReferenceExpression();
            codeRefExp.MethodName = field.Name + ".Add";
            CodeMethodInvokeExpression invoke =
                new CodeMethodInvokeExpression(
                    codeRefExp,
                    new CodeVariableReferenceExpression("strClassName"),
                    new CodeVariableReferenceExpression("classType")
                );

            setMethod_privateSet.Statements.Add(invoke);*/
        }

        public void BuildBaseManagerClass()
        {
            unit_manager_base = null;
            unit_manager_base = new CodeCompileUnit();

            customNameSpace_manager.Imports.Add(new CodeNamespaceImport("UnityEngine"));
            customNameSpace_manager.Imports.Add(new CodeNamespaceImport("System.Collections.Generic"));
            customNameSpace_manager.Imports.Add(new CodeNamespaceImport("System"));
            unit_manager.Namespaces.Add(customNameSpace_manager);

            tableClass_manager_base = new CodeTypeDeclaration("TableLoaderBase");
            tableClass_manager_base.IsClass = true;
            tableClass_manager_base.TypeAttributes = TypeAttributes.Public;
            customNameSpace_manager.Types.Add(tableClass_manager_base);

            CodeMemberField field = new CodeMemberField(typeof(string), "m_strTableName");
            field.Attributes = MemberAttributes.Public;
            tableClass_manager_base.Members.Add(field);
        }

        public void AddManagerMember(string className)
        {
            CodeMemberField configTable_manager = new CodeMemberField("Table" + className, "m_table" + className);
            configTable_manager.Attributes = MemberAttributes.Public | MemberAttributes.Static;
            tableClass_manager.Members.Add(configTable_manager);
        }

        public void AddStaticManagerMember(string className)
        {
            CodeMemberField configTable_manager = new CodeMemberField("Table" + className, "m_table" + className);
            configTable_manager.Attributes = MemberAttributes.Public | MemberAttributes.Static;
            tableClass_manager.Members.Add(configTable_manager);
        }

        public void BuildCustomClass(string strClassName)
		{
			unit = null;
            customerclass = null;
			tableClass = null;
			configTable = null;
			m_filed.Clear ();
			m_typeSave.Clear ();

			unit = new CodeCompileUnit();
            customNameSpace = new CodeNamespace("XWorld.DataConfig");
			customNameSpace.Imports.Add(new CodeNamespaceImport("UnityEngine"));
			customNameSpace.Imports.Add(new CodeNamespaceImport("System.Collections.Generic"));
            customNameSpace.Imports.Add(new CodeNamespaceImport("System"));
            unit.Namespaces.Add(customNameSpace);
            
            //自定义的同名数据类成员
            tableClass = new CodeTypeDeclaration(strClassName);
            tableClass.BaseTypes.Add(new CodeTypeReference("System.ICloneable"));
            tableClass.IsClass = true;
			tableClass.TypeAttributes = TypeAttributes.Public;
            customNameSpace.Types.Add(tableClass);

            setMethod_manager = new CodeMemberMethod();
            setMethod_manager.Attributes = MemberAttributes.Public | MemberAttributes.Final;
            setMethod_manager.Name = "Clone";
            setMethod_manager.ReturnType = new CodeTypeReference(typeof(object).ToString());
            tableClass.Members.Add(setMethod_manager);
            setMethod_manager.Statements.Add(new CodeVariableReferenceExpression("return this.MemberwiseClone();"));

            //根据txt生成的同名文件
            customerclass = new CodeTypeDeclaration("Table" + strClassName);
			customerclass.IsClass = true;
            customerclass.TypeAttributes = TypeAttributes.Public;
            customerclass.BaseTypes.Add(new CodeTypeReference("XWorld.DataConfig.TableBase"));
            customNameSpace.Types.Add(customerclass);

			//生成List
			configTable = new CodeMemberField ("List<" + tableClass.Name + ">","m_configList");
			configTable.Attributes = MemberAttributes.Public;
			configTable.InitExpression = new CodeObjectCreateExpression ("List<" + tableClass.Name + ">");
			customerclass.Members.Add (configTable);

			//定义类方法
			setMethod = new CodeMemberMethod();
			setMethod.Attributes = MemberAttributes.Public | MemberAttributes.Final;
			customerclass.Members.Add (setMethod);

			setMethod.Name = "LoadData";
			CodeParameterDeclarationExpression param1 = new CodeParameterDeclarationExpression ("XWorld.DataConfig."+ strClassName, "codeValue");
			setMethod.Parameters.Add (param1);

			CodeMethodReferenceExpression codeRefExp = new CodeMethodReferenceExpression ();
			codeRefExp.MethodName = configTable.Name + ".Add"; 
			CodeMethodInvokeExpression invoke = 
				new CodeMethodInvokeExpression (
					codeRefExp,
					new CodeVariableReferenceExpression("codeValue")
				);
					
			setMethod.Statements.Add (invoke);
		}

        public void BuildCustomClassStepLoad(string strClassName, string[] typeNames, string[] memberNames,bool bDescTips)
        {
            //Get Row Count
            CodeMemberMethod rowMethod = new CodeMemberMethod();
            rowMethod.Attributes = MemberAttributes.Public | MemberAttributes.Final;
            rowMethod.Name = "GetRowCount";
            rowMethod.ReturnType = new CodeTypeReference(typeof(int).ToString());
            customerclass.Members.Add(rowMethod);
            rowMethod.Statements.Add(new CodeVariableReferenceExpression("return " + memberNames.Length.ToString()));

			//GetDataCount
			CodeMemberMethod dataCountMethod = new CodeMemberMethod();
			dataCountMethod.Attributes = MemberAttributes.Public | MemberAttributes.Final;
			dataCountMethod.Name = "GetDataCount";
			dataCountMethod.ReturnType = new CodeTypeReference(typeof(int));
			CodeVariableReferenceExpression dataCountCheck = new CodeVariableReferenceExpression(configTable.Name.ToString() + " == null");
			CodeConditionStatement codeStateDataCount = new CodeConditionStatement();
			codeStateDataCount.Condition = dataCountCheck;
			codeStateDataCount.TrueStatements.Add (new CodeVariableReferenceExpression ("return 0"));
			dataCountMethod.Statements.Add (codeStateDataCount);
			dataCountMethod.Statements.Add (new CodeVariableReferenceExpression ("return " + configTable.Name.ToString() + ".Count"));
			customerclass.Members.Add (dataCountMethod);

			//GetData
			CodeMemberMethod getDataMethod = new CodeMemberMethod();
			getDataMethod.Attributes = MemberAttributes.Public | MemberAttributes.Final;
			getDataMethod.Name = "GetData";
			getDataMethod.ReturnType = new CodeTypeReference(strClassName);
			CodeParameterDeclarationExpression paramGetData1 = new CodeParameterDeclarationExpression(typeof(int), "rowIdx");
			getDataMethod.Parameters.Add(paramGetData1);
			CodeVariableReferenceExpression getDataCheckCode = new CodeVariableReferenceExpression(configTable.Name.ToString() + " != null && " + "rowIdx >= 0 && rowIdx < " + configTable.Name.ToString() + ".Count");
			CodeConditionStatement codstateGet = new CodeConditionStatement();
			codstateGet.Condition = getDataCheckCode;
			codstateGet.TrueStatements.Add (new CodeVariableReferenceExpression ("return " + configTable.Name.ToString() + "[rowIdx]"));
			getDataMethod.Statements.Add (codstateGet);
			getDataMethod.Statements.Add (new CodeVariableReferenceExpression ("return null"));
			customerclass.Members.Add (getDataMethod);

            //Load Data
            setMethod = new CodeMemberMethod();
            setMethod.Attributes = MemberAttributes.Public | MemberAttributes.Override;
            customerclass.Members.Add(setMethod);

            setMethod.Name = "LoadData";
            CodeParameterDeclarationExpression param1 = new CodeParameterDeclarationExpression(typeof(string), "content");
            setMethod.Parameters.Add(param1);

         //   setMethod.Statements.Add(new CodeTypeReferenceExpression("content = content.Trim(ClientConfigManager.CMD_CHAR)"));

            CodeMethodReferenceExpression codeRefExp = new CodeMethodReferenceExpression();
            codeRefExp.MethodName = @"string[] values = content.Split";

            CodeMethodInvokeExpression invoke =
                new CodeMethodInvokeExpression(
                    codeRefExp,
                    new CodeVariableReferenceExpression("\"\\r\"[0]")
                );
            setMethod.Statements.Add(invoke);

            int initLineNum = 2;
            if(bDescTips)
            {
                initLineNum = 3;
            }

            CodeIterationStatement its = new CodeIterationStatement();
            // 初始化条件
            its.InitStatement = new CodeVariableDeclarationStatement(typeof(int), "i", new CodePrimitiveExpression(initLineNum));
            // 条件检查
            its.TestExpression = new CodeBinaryOperatorExpression(new CodeVariableReferenceExpression("i"), CodeBinaryOperatorType.LessThan, new CodeTypeReferenceExpression("values.Length"));
            // 每一轮循环后对条件的更改
            its.IncrementStatement = new CodeAssignStatement(new CodeVariableReferenceExpression("i"), new CodeBinaryOperatorExpression(new CodeVariableReferenceExpression("i"), CodeBinaryOperatorType.Add, new CodePrimitiveExpression(1)));
            
            // 循环体
            {
               
                CodeAssignStatement ass = new CodeAssignStatement();
                ass.Left = new CodeArgumentReferenceExpression(strClassName + " data");
                ass.Right = new CodeArgumentReferenceExpression("new " + strClassName + "()");
                its.Statements.Add(ass);

                its.Statements.Add(new CodeTypeReferenceExpression("int j = 0"));

                codeRefExp = new CodeMethodReferenceExpression();
                codeRefExp.MethodName = @"string[] subValues = values[i].TrimStart('\n').Split";

                invoke =
                  new CodeMethodInvokeExpression(
                      codeRefExp,
                      new CodeVariableReferenceExpression("ClientConfigManager.CMD_STRING"),
                      new CodeVariableReferenceExpression("StringSplitOptions.None")
                  );
                its.Statements.Add(invoke);
                
                // 生成判断条件的表达式
                CodeVariableReferenceExpression codexp = new CodeVariableReferenceExpression("subValues != null && subValues.Length == GetRowCount()");
                // 分支语句
                CodeConditionStatement codstatement = new CodeConditionStatement();
                codstatement.Condition = codexp;
                // 条件成立时
               // codstatement.TrueStatements.Add(new CodeTypeReferenceExpression());
                its.Statements.Add(codstatement);

                for (int i = 0; i < typeNames.Length; i++)
                {
                    string typeName = typeNames[i];
                    string memberName = memberNames[i];


                    codeRefExp = new CodeMethodReferenceExpression();

                    codeRefExp.MethodName = @"data." + memberName + " = " + GetConvertByType(typeName);

                    invoke =
                        new CodeMethodInvokeExpression(
                            codeRefExp,
                            new CodeVariableReferenceExpression("subValues[j]")
                        );
                    //  its.Statements.Add(invoke);
                    codstatement.TrueStatements.Add(invoke);

                    ass = new CodeAssignStatement();
                    ass.Left = new CodeArgumentReferenceExpression("j");
                    ass.Right = new CodeArgumentReferenceExpression("j + 1");
                    //  its.Statements.Add(ass);
                    codstatement.TrueStatements.Add(ass);
                }
                codeRefExp = new CodeMethodReferenceExpression();
                codeRefExp.MethodName = @"m_configList.Add";

                invoke =
                    new CodeMethodInvokeExpression(
                        codeRefExp,
                        new CodeVariableReferenceExpression("data")
                    );

                codstatement.TrueStatements.Add(invoke);

            }

            setMethod.Statements.Add(its);
        }

        private static string GetConvertByType(string strTypeName)
        {
            if (strTypeName.Contains("_key") || strTypeName.Contains("_Key"))
            {
                string[] arr = strTypeName.Split('_');
                if(arr.Length > 1)
                {
                    strTypeName = arr[0];
                }
            }

            string field = null;
            if (strTypeName.CompareTo("char") == 0)
            {

            }
            if (strTypeName.CompareTo("bool") == 0)
            {
                field = "ClientConfigManager.ToBoolean";
            }
            if (strTypeName.CompareTo("int16") == 0)
            {
                field = "ClientConfigManager.ToInt16";
            }
            if (strTypeName.CompareTo("int32") == 0)
            {
                field = "ClientConfigManager.ToInt32";
            }
            if (strTypeName.CompareTo("int64") == 0)
            {
                field = "ClientConfigManager.ToInt64";
            }
            if (strTypeName.CompareTo("uint16") == 0)
            {
                field = "ClientConfigManager.ToUInt16";
            }
            if (strTypeName.CompareTo("uint32") == 0)
            {
                field = "ClientConfigManager.ToUInt32";
            }
            if (strTypeName.CompareTo("uint64") == 0)
            {
                field = "ClientConfigManager.ToUInt64";
            }
            if (strTypeName.CompareTo("f32") == 0)
            {
                field = "ClientConfigManager.ToSingle";
            }
            if (strTypeName.CompareTo("f64") == 0)
            {
                field = "ClientConfigManager.ToDouble";
            }

            return field;
        }


        public void CodeMethon_KeyPair(CodeMemberField key,CodeMemberField value)
		{
			if (key == null || value == null)
				return;

			if (configTable == null)
				return;

			return;
			CodeAssignStatement ass = new CodeAssignStatement ();
			ass.Left = new CodeIndexerExpression(
				new CodeVariableReferenceExpression(configTable.Name),
				new CodePrimitiveExpression(key));
			
			ass.Right = new CodeIndexerExpression(
				new CodeVariableReferenceExpression(configTable.Name),
				new CodePrimitiveExpression(value)); 
		}

		public void Code_CreateCSharp(string strFileName,bool bDynamic)
		{
			CodeDomProvider proivder = CodeDomProvider.CreateProvider ("CSharp");
			CodeGeneratorOptions options = new CodeGeneratorOptions ();
			options.BracingStyle = "C";
			options.BlankLinesBetweenMembers = true;

            string str = "";
            str = bDynamic ? Application.dataPath + "/Scripts/DataClient/Config/Dynamic/" + strFileName + ".cs" 
                    : Application.dataPath + "/Scripts/DataClient/Config/Static/" + strFileName + ".cs";

			Debug.Log ("Final Path : " + str);
            using (StreamWriter sw = new StreamWriter(str))
            {
                if (sw != null)
                {
                    proivder.GenerateCodeFromCompileUnit(unit, sw, options);
                }
            }
		}

        public void Code_CreateManagerCSharp()
        {
            CodeDomProvider proivder = CodeDomProvider.CreateProvider("CSharp");
            CodeGeneratorOptions options = new CodeGeneratorOptions();
            options.BracingStyle = "C";
            options.BlankLinesBetweenMembers = true;

            string str = Application.dataPath + "/Scripts/DataClient/ClientConfigManager.Init.cs";
            Debug.Log("Final Path : " + str);
            using (StreamWriter sw = new StreamWriter(str))
            {
                if (sw != null)
                {
                    proivder.GenerateCodeFromCompileUnit(unit_manager, sw, options);
                }
            }
        }
    }

    private static readonly string strPath = @"\AssetDatas\Config\Dynamic\ClientData";
	private static readonly string strParamPath = @"\AssetDatas\Config\Dynamic\ParamData";
    private static readonly string strStaticPath = @"\AssetDatas\Config\Static";
	private static readonly string strPattern = "*.txt";
	private static readonly string[] CMD_STRING = { "\n", "\t" };

	private static CSharpCalssBuilder m_builder = new CSharpCalssBuilder();

	public static bool IsBuilding
	{
		get;
		set;
	}

	public static void InitAutoClassBuilder()
	{
		if (m_builder == null)
			m_builder = new CSharpCalssBuilder ();

		IsBuilding = false;
	}

    private static List<string> m_allTxtConfig = new List<string>();
	public static void CopyAllTxt()
	{
		//删除原有文件夹文件 ClientData
		DirectoryInfo info = new DirectoryInfo(Application.dataPath + strPath);
		FileInfo[] infos = info.GetFiles(strPattern);
		if (infos != null) 
		{
			foreach (var item in infos) 
			{
				if (item == null)
					continue;

				item.Delete ();
			}
		}
		//删除ParamData
		DirectoryInfo infoParam = new DirectoryInfo(Application.dataPath + strParamPath);
		FileInfo[] infosinfoParam= infoParam.GetFiles(strPattern);
		if (infosinfoParam != null) 
		{
			foreach (var item in infosinfoParam) 
			{
				if (item == null)
					continue;

				item.Delete ();
			}
		}

		AssetDatabase.Refresh ();

		DirectoryInfo t_tempInfo = new DirectoryInfo (Application.dataPath);
		t_tempInfo = t_tempInfo.Parent;
		t_tempInfo = t_tempInfo.Parent;
		//t_tempInfo = t_tempInfo.Parent;

		string strSrcPath = t_tempInfo.ToString() + "/Server/bin32/Data/DataTable";
		strSrcPath = strSrcPath.Replace('/','\\');
		if (!Directory.Exists (strPath)) 
		{
			Directory.CreateDirectory (strPath);
		}

		if (!Directory.Exists (strParamPath)) 
		{
			Directory.CreateDirectory (strParamPath);
		}

		AssetDatabase.Refresh ();


        string allTxtsConfigPath = Application.dataPath + "/AssetDatas/Config/AllTxtConfigs.txt";
        string[]  configFile = Directory.GetFiles(allTxtsConfigPath);
        if(configFile != null)
        {
            FileStream fs = new FileStream(allTxtsConfigPath, FileMode.Open, FileAccess.Read, FileShare.None);
            if (fs == null)
            {
                Debug.LogError("allTxtConfigs file is not exsit!");
                return;
            }

            StreamReader sr = new StreamReader(fs);
            if (sr != null)
            {
                string strContent = sr.ReadToEnd();

                string[] strLines = strContent.Split("\r"[0]);
                if (strLines == null)
                {
                    Debug.LogError("allTxtConfigs file parse failed!");
                    return;
                }

                //第一行是表头
                for(int i = 1; i < strLines.Length;++i)
                {
                    if (strLines[i] == "\n")
                        continue;

                    string[] typeLines = strLines[i].Split(CMD_STRING, StringSplitOptions.RemoveEmptyEntries);
                    m_allTxtConfig.Add(typeLines[0] + ".txt");
                }
            }
        }

        //参数集以外的表复制到ConfigData
        string[] fileList = Directory.GetFiles (strSrcPath);
		foreach (var item in fileList) 
		{
			string[] strNames = item.Split ('\\');

			if (strNames[strNames.Length - 1].StartsWith ("Param") || strNames[strNames.Length - 1].StartsWith ("param")) 
			{
				string trueName = Application.dataPath + strParamPath + '/' + strNames [strNames.Length - 1];
				FileInfo file = new FileInfo (item);

                using (FileStream fs = File.Create (trueName)) 
				{
                    
                }
                file.CopyTo(trueName, true);
            } 
			else 
			{
				string trueName = Application.dataPath + strPath + '/' + strNames[strNames.Length - 1];
                if (!m_allTxtConfig.Contains(strNames[strNames.Length - 1]))
                    continue;

				FileInfo file = new FileInfo (item);

				using (FileStream fs = File.Create (trueName)) 
				{
                }

                file.CopyTo(trueName, true);
                m_allTxtConfig.Remove(strNames[strNames.Length - 1]);
            }

		}

        AssetDatabase.Refresh ();
        m_allTxtConfig.Clear();
    }

	public static void DoAuto()
    {
        AutoDynamic();
        AutoStatic();
    }

    private static void AutoDynamic()
    {
        string strFileConfig = AutoCSharpToolPath.PATH_INIT_LOADER_FILES;
        if (File.Exists(strFileConfig))
        {
            File.Delete(strFileConfig);
        }

        DirectoryInfo lastFileInfo = new DirectoryInfo(AutoCSharpToolPath.PATH_STATIC_C_SHARP_FILES);
        FileInfo[] csFiles = lastFileInfo.GetFiles("*.cs");
        foreach (FileInfo item in csFiles)
        {
            item.Delete();
        }
        AssetDatabase.Refresh();

        try
        {
            IsBuilding = true;
            DirectoryInfo info = new DirectoryInfo(Application.dataPath + strPath);
            FileInfo[] infos = info.GetFiles(strPattern);

            m_builder.BuildManagerClass();
            //m_builder.BuildBaseManagerClass();

            foreach (var item in infos)
            {
                if (item.Name.Contains("meta"))
                    continue;

                if (item.Name.Contains("order") || item.Name.Contains("where"))
                {
                    Debug.Log("File name error! name : " + item.Name);
                    continue;
                }

                string[] strName = item.Name.Split('.');

                string strContent = ParseFiles(item.FullName);
                string strFileName = strName[0];
                string[] strLines = strContent.Split("\r"[0]);
                if (strLines == null)
                {
                    //Debug.LogError();
                    continue;
                }

                string strSubString = strContent.Substring(0,1);
                bool bDesc = strSubString.Equals("#");

                Debug.Log("CreateFile : " + strFileName + "");

                m_builder.AddManagerMember(strFileName);
                m_builder.BuildCustomClass(strFileName);

                m_builder.m_filed.Clear();
                m_builder.m_typeSave.Clear();

                int initLine = 1;
                if(bDesc)
                {
                    initLine = 2;
                }

                string[] typeLines = strLines[initLine].Split(CMD_STRING, StringSplitOptions.RemoveEmptyEntries);
                if (typeLines == null)
                {
                    Debug.LogError("Title type is error! className : " + strFileName);
                    break;
                }

                for (int x = 0; x < typeLines.Length; ++x)
                {
                    m_builder.m_typeSave.Add(typeLines[x].ToLower());
                }

                TryParseTitle(0, m_builder, strLines[initLine - 1], strFileName);
                string[] memberLines = strLines[initLine - 1].Split(CMD_STRING, StringSplitOptions.RemoveEmptyEntries);
                m_builder.BuildCustomClassStepLoad(strFileName, typeLines, memberLines,bDesc);

                m_builder.Code_CreateCSharp(strFileName,true);
            }

            m_builder.Code_CreateManagerCSharp();
            IsBuilding = true;
            AssetDatabase.Refresh();
            Debug.Log("CreateFileEnd! ");
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
            IsBuilding = false;
        }
    }

    private static void AutoStatic()
    {
        DirectoryInfo lastFileInfo = new DirectoryInfo(AutoCSharpToolPath.PATH_STATIC_C_SHARP_FILES);
        FileInfo[] csFiles = lastFileInfo.GetFiles("*.cs");
        foreach (FileInfo item in csFiles)
        {
            item.Delete();
        }

        AssetDatabase.Refresh();

        try
        {
            DirectoryInfo info = new DirectoryInfo(Application.dataPath + strStaticPath);
            FileInfo[] infos = info.GetFiles(strPattern);

            foreach (var item in infos)
            {
                if (item.Name.Contains("meta"))
                    continue;

                if (item.Name.Contains("order") || item.Name.Contains("where"))
                {
                    Debug.Log("File name error! name : " + item.Name);
                    continue;
                }

                string[] strName = item.Name.Split('.');

                string strContent = ParseFiles(item.FullName);
                string strFileName = strName[0];
                string[] strLines = strContent.Split("\r"[0]);
                if (strLines == null)
                {
                    //Debug.LogError();
                    continue;
                }

                Debug.Log("CreateFile : " + strFileName + "");

                m_builder.AddStaticManagerMember(strFileName);
                m_builder.BuildCustomClass(strFileName);

                m_builder.m_filed.Clear();
                m_builder.m_typeSave.Clear();

                string strSubString = strContent.Substring(0,1);
                bool bDesc = strSubString.Equals("#");

                int initLine = 1;
                if (bDesc)
                {
                    initLine = 2;
                }

                string[] typeLines = strLines[initLine].Split(CMD_STRING, StringSplitOptions.RemoveEmptyEntries);
                if (typeLines == null)
                {
                    Debug.LogError("Title type is error! className : " + strFileName);
                    break;
                }

                for (int x = 0; x < typeLines.Length; ++x)
                {
                    m_builder.m_typeSave.Add(typeLines[x].ToLower());
                }

                TryParseTitle(0, m_builder, strLines[initLine-1], strFileName);
                string[] memberLines = strLines[initLine - 1].Split(CMD_STRING, StringSplitOptions.RemoveEmptyEntries);
                m_builder.BuildCustomClassStepLoad(strFileName, typeLines, memberLines, bDesc);

                m_builder.Code_CreateCSharp(strFileName,false);
            }

            m_builder.Code_CreateManagerCSharp();
            IsBuilding = true;
            AssetDatabase.Refresh();
            Debug.Log("CreateFileEnd! ");
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
            IsBuilding = false;
        }
    }

    private static string ParseFiles(string strPath)
    {
        string strContent = "";
        FileStream fs = new FileStream(strPath, FileMode.Open, FileAccess.Read, FileShare.None);
        if (fs != null)
        {
            StreamReader sr = new StreamReader(fs);
            if(sr != null)
            {
                strContent = sr.ReadToEnd();
            }
        }

        return strContent;
    }

	private static bool TryParseTitle(int idx,CSharpCalssBuilder builder, string strMemberLine,string strClassName)
	{
		if (builder == null)
			return false;

		string[] memberLines = strMemberLine.Split(CMD_STRING,StringSplitOptions.RemoveEmptyEntries);
		if (memberLines == null)
			return false;

		CodeMemberField field = null;
		for (int i = 0; i < memberLines.Length; ++i) 
		{
			memberLines [i].ToLower ();
			field = GetFieldByType (memberLines [i],m_builder.m_typeSave[i]);

			if (field == null) 
			{
				Debug.LogError ("未知类型定义！标题解析失败! 文件名称 : " + strClassName);
				return false;
			}

			field.Attributes = MemberAttributes.Public;
			m_builder.m_filed.Add(field);
			builder.tableClass.Members.Add(field);
		}

		return true;
	}

	private static CodeMemberField GetFieldByType(string strFiledName,string strTypeName)
	{
		CodeMemberField field = null;
		if (strTypeName.CompareTo ("char") == 0) 
		{
			field = new CodeMemberField(typeof(string), strFiledName);
		}
		if (strTypeName.CompareTo ("bool") == 0) 
		{
			field = new CodeMemberField(typeof(bool), strFiledName);
		}
		if (strTypeName.CompareTo ("int16") == 0) 
		{
			field = new CodeMemberField(typeof(short), strFiledName);
		}
		if (strTypeName.CompareTo ("int32") == 0) 
		{
			field = new CodeMemberField(typeof(int), strFiledName);
		}
		if (strTypeName.CompareTo ("int64") == 0) 
		{
			field = new CodeMemberField(typeof(long), strFiledName);
		}
		if (strTypeName.CompareTo ("uint16") == 0) 
		{
			field = new CodeMemberField(typeof(ushort), strFiledName);
		}
		if (strTypeName.CompareTo ("uint32") == 0) 
		{
			field = new CodeMemberField(typeof(uint), strFiledName);
		}
		if (strTypeName.CompareTo ("uint64") == 0) 
		{
			field = new CodeMemberField(typeof(ulong), strFiledName);
		}
		if (strTypeName.CompareTo ("f32") == 0) 
		{
			field = new CodeMemberField(typeof(float), strFiledName);
		}
		if (strTypeName.CompareTo ("f64") == 0) 
		{
			field = new CodeMemberField(typeof(double), strFiledName);
		}

		return field;
	}

	private static CodeAssignStatement SetFieldValue(CodeMemberField codeFiled,string strValueType, string strValue)
	{
		if (codeFiled == null)
			return null;

		bool bFind = false;
		CodeAssignStatement ass = new CodeAssignStatement ();

		if (strValueType.CompareTo ("char") == 0) 
		{
			ass.Right = new CodePrimitiveExpression (strValue);
			bFind = true;
		}
		if (strValueType.CompareTo ("bool") == 0) 
		{
			bool val = false;
			if (bool.TryParse (strValue, out val)) 
			{
				ass.Right = new CodePrimitiveExpression (val);
				bFind = true;
			}
		}
		if (strValueType.CompareTo ("int16") == 0) 
		{
			short val = 0;
			if (short.TryParse (strValue, out val)) 
			{
				ass.Right = new CodePrimitiveExpression (val);
				bFind = true;
			}
		}
		if (strValueType.CompareTo ("int32") == 0) 
		{
			int val = 0;
			if (int.TryParse (strValue, out val)) 
			{
				ass.Right = new CodePrimitiveExpression (val);
				bFind = true;
			}
		}
		if (strValueType.CompareTo ("int64") == 0) 
		{
			long val = 0;
			if (long.TryParse (strValue, out val)) 
			{
				ass.Right = new CodePrimitiveExpression (val);
				bFind = true;
			}
		}
		if (strValueType.CompareTo ("uint16") == 0) 
		{
			ushort val = 0;
			if (ushort.TryParse (strValue, out val)) 
			{
				ass.Right = new CodePrimitiveExpression (val);
				bFind = true;
			}
		}
		if (strValueType.CompareTo ("uint32") == 0) 
		{
			uint val = 0;
			if (uint.TryParse (strValue, out val)) 
			{
				ass.Right = new CodePrimitiveExpression (val);
				bFind = true;
			}
		}
		if (strValueType.CompareTo ("uint64") == 0) 
		{
			uint val = 0;
			if (uint.TryParse (strValue, out val)) 
			{
				ass.Right = new CodePrimitiveExpression (val);
				bFind = true;
			}
		}
		if (strValueType.CompareTo ("float") == 0) 
		{
			float val = 0;
			if (float.TryParse (strValue, out val)) 
			{
				ass.Right = new CodePrimitiveExpression (val);
				bFind = true;
			}
		}
		if (strValueType.CompareTo ("double") == 0) 
		{
			double val = 0;
			if (double.TryParse (strValue, out val)) 
			{
				ass.Right = new CodePrimitiveExpression (val);
				bFind = true;
			}
		}

		if (bFind) 
		{
			ass.Left = new CodeArgumentReferenceExpression (codeFiled.Name);
			return ass;
		}

		return null;
	}

	private static bool GetConfigValue(CSharpCalssBuilder builder, string strMemberLine,string strClassName)
    {
		if (builder == null)
			return false;

        string[] memberLines = strMemberLine.Split(CMD_STRING,StringSplitOptions.RemoveEmptyEntries);
        if (memberLines == null)
			return false;

		for (int i = 0; i < memberLines.Length; ++i) 
		{
			CodeAssignStatement ass = SetFieldValue (m_builder.m_filed [i], m_builder.m_typeSave [i], memberLines [i]);
			if (ass == null) 
			{
				Debug.LogError ("Error Field Value! : class : " + strClassName + " Filed : " + m_builder.m_filed [i].ToString() + "  value : " + memberLines[i].ToString());
				return false;
			}
		}
			
		/*CodeAssignStatement t_ass = new CodeAssignStatement ();
		t_ass.Left = new CodeIndexerExpression(
			new CodeVariableReferenceExpression(builder.configTable.Name),
			new CodePrimitiveExpression(m_builder.m_filed [0]));

		t_ass.Right = new CodeIndexerExpression(
			new CodeVariableReferenceExpression(builder.configTable.Name),
			new CodePrimitiveExpression(m_builder.tableClass)); */

		//m_builder.CodeMethon_KeyPair (m_builder.m_filed [0],m_builder.configTable);
		return true;
    }

    public static string LowerFirst(string s)
    {
        return Regex.Replace(s, @"\b[A-Z]\w+", delegate (Match match)
        {
            string v = match.ToString();
            return char.ToLower(v[0]) + v.Substring(1);
        });
    }
}
