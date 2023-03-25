using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace Yukari
{
    /// <summary>
    /// 状态机，状态系统
    /// </summary>
    public class StateMachine
    {
        #region 属性
        /// <summary>
        /// 系统中的单元
        /// </summary>
        private Dictionary<string, StatusCell> cells = new Dictionary<string, StatusCell>();
        public readonly List<string> pathInfo = new List<string>();//脚本路径信息
        #endregion

        #region 响应函数
        /// <summary>
        /// 判断指定状态是否存在
        /// </summary>
        /// <param name="key">状态名称</param>
        /// <returns></returns>
        public bool isExist(string key)
        {
            foreach (string k in cells.Keys)
            {
                if (k == key) { return true; }
            }
            return false;
        }
        /// <summary>
        /// 获取指定状态信息
        /// </summary>
        /// <param name="name">状态名称</param>
        /// <returns>若不存在则返回null</returns>
        public StatusCell Find(string name, int index = 0)
        {
            if (isExist(name)) { return cells[name].Copy(index); }
            if (isExist("End")) { return cells["End"].Copy(index); }
            return new StatusCell("End");
        }
        /// <summary>
        /// 向库中添加新的状态，同名状态则覆盖原先状态
        /// </summary>
        /// <param name="cell">新添状态单元对象</param>
        public void Add(StatusCell cell)
        {
            if (isExist(cell.name)) { cells[cell.name] = cell; }
            else { cells.Add(cell.name, cell); }
        }
        /// <summary>
        /// 清空库存
        /// </summary>
        public void Clear()
        {
            cells.Clear();
            pathInfo.Clear();
        }
        /// <summary>
        /// 打印全部状态信息
        /// </summary>
        public void LogAll()
        {
            foreach (StatusCell c in cells.Values)
            {
                c.PrintInfo();
                Console.WriteLine();
            }
        }
        /// <summary>
        /// 清除null元素/属性
        /// </summary>
        public void ClearNull()
        {
            foreach (StatusCell c in cells.Values)
            {
                c.ClearNull();
            }
        }
        #endregion


    }

    /// <summary>
    /// 脚本解释器
    /// </summary>
    public class ScriptParser
    {
        private enum formCell { waitCname, Cname, Cnameok, step, stepok }
        private enum formStep { waitSname, Sname, Snameok, param, paramok, waitTo, to, took, go }
        private enum formParam { waitKname, Kname, Knameok, waitVname, Vname, Vnameok }
        private enum formTo { waitTname, Tname, Tnameok, waitIname, Iname, Inameok, waitIndex, Index, Indexok, waitFunction, function, functionok }
        private enum formFunc { waitFname, Fname, Fnameok, param, paramok }

        private enum formCmd { non, waitName, Name, _Name, Nameok, param, paramok }

        #region 构造函数
        public ScriptParser() { param_cmd = new Dictionary<string, Dictionary<string, object>>(); }
        public ScriptParser(ScriptParser sp)
        {
            param_cmd = sp.param_cmd;
        }
        #endregion


        #region 状态器
        private formCell cell = formCell.waitCname;
        private formStep step = formStep.waitSname;
        private formParam param = formParam.waitKname;
        private formTo to = formTo.waitTname;
        private formFunc func = formFunc.waitFname;
        private formCmd cmd = formCmd.non;
        #endregion

        #region 缓存器
        private int charcount = 0;//计数器
        private int rowcount = 1;//行计数器
        private Dictionary<string, Dictionary<string, object>> param_cmd;
        private Dictionary<string, object> param_cmd_c;//参数临时缓存器
        private string cache_cmd;//指令缓存
        private string cache;//命名缓存
        private string cache2;//命名缓存
        private StatusCell cell_c;
        private StepInfo step_c;
        private Dictionary<string, object> param_c;
        private SkipInfo skip_c;
        private FunctionInfo function_c;
        private StateMachine sm;
        #endregion

        private bool innote;//注释模式
        private bool inchange;//转义编辑
        private bool inquote;//引用编辑

        /// <summary>
        /// 将特定文本转化为对应的类型值
        /// </summary>
        /// <param name="txt"></param>
        /// <returns></returns>
        private object Changevalue(string txt)
        {
            object value = null;
            if (Regex.IsMatch(cache2, @"^(0|-?[1-9][0-9]*)$"))//判断是否为整形
            {
                value = Convert.ToInt32(cache2);
            }
            else if (Regex.IsMatch(cache2, @"^(-?(0|[1-9][0-9]*))(\.\d+)?$"))//判断是否为实形
            {
                value = Convert.ToDouble(cache2);
            }
            else
            {
                if (txt.ToLower() == "true")
                {
                    value = true;
                }
                else if (txt.ToLower() == "false")
                {
                    value = false;
                }
                else
                {
                    value = txt;
                }
            }
            return value;

        }

        private bool inKey(string key,Dictionary<string, Dictionary<string, object>> dic)
        {
            foreach(string k in dic.Keys)
            {
                if (key == k) { return true; }
            }
            return false;
        }
        #region 指令
        private void cmd_import(Dictionary<string, object> p)//加载脚本指令
        {
            string path = p["path"] + "";
            foreach(string u in sm.pathInfo)//避免重复加载同一脚本文件
            {
                if (u == path) { return; }
            }
            sm.pathInfo.Add(path);
            ScriptParser sp = new ScriptParser(this);
            sp.LoadFormFile(path, sm);
    }
        private void cmd_pack(Dictionary<string, object> p)//封装参数包
        {
            Dictionary<string, object> pack = new Dictionary<string, object>();
            param_cmd.Add(p["name"] + "", pack);
            foreach (string key in p.Keys)
            {
                if (key != "name") { pack.Add(key, p[key]); }
            }
        }
        private void cmd_up(Dictionary<string, object> p)//卸载参数包
        {
            if (param == formParam.waitKname)
            {
                param_c = param_cmd[p["name"] + ""];
                cmd = formCmd.non;
                param = formParam.Vnameok;
            }
        }

        private void cmd_erro(string log,char c)
        {
            Console.WriteLine(log);
            Console.Write($"错误：第{rowcount}行,第{charcount}个字符:\t{c}");
        }
        #endregion

        /**
         * 引号的作用是，表示一个整体
         * 在引号编辑内是，转义才起作用
         */
        /// <summary>
        /// 从指定脚本文件中解析指令到指定状态机对象
        /// </summary>
        /// <param name="fileName">脚本文件路径（包含后缀.ykr）</param>
        /// <param name="sm">实现指令的状态机对象</param>
        /// <return>若无法成功解析，或者解析出错，则终止返回false，正常解析完毕返回true</return>
        public bool LoadFormFile(string fileName, StateMachine sm_)
        {
            sm = sm_;
            if (!File.Exists(fileName)) { return false; }
            sm.pathInfo.Add(fileName);
            StreamReader reader = new StreamReader(fileName);

            char[] buffer = new char[1024];//缓存
            int length = reader.Read(buffer);//读取长度
            while (length > 0)
            {
                if (!Parse(buffer, length))//解析异常直接终止
                {
                    Console.WriteLine("解析异常");
                    sm.Clear();
                    return false;
                }
                length = reader.Read(buffer);
            }
            //所有文本解析完毕后
            reader.Close();
            if (cell != formCell.waitCname)//存在编辑器状态则说明解析异常
            {
                Console.WriteLine("终止解析异常");
                sm.Clear();
                return false;
            }
            sm.ClearNull();//清理为null的对象
            return true;
        }

        public bool Parse(char[] input, int length)
        {
            int i = 0;
            bool erro = false;
            while (i < length)
            {
                char c = input[i];
                ++charcount;
                if (c == '\n') { ++rowcount; charcount = 0; }
                if (cmd == formCmd.non)
                {
                    if (c == '#' && !inquote) { cmd = formCmd.waitName; cache_cmd = ""; }//清空指令名称缓存
                    else{ if (!Parse_Cell(c)) { erro = true;} }

                }
                else
                {
                    if (c == '\r' || c == '\n') { cmd = formCmd.non; innote = false; }
                    else { if (!Parse_cmd(c)) { erro = true; } }

                }
                ++i;
                if (erro) 
                {
                    Console.Write(c);
                    if (c == '\n') { erro = false; }
                }
            }
            return true;
        }

        private bool Parse_cmd(char c)//解析解释器指令
        {
            if (innote) { return true; }
            switch (cmd)
            {
                case formCmd.waitName:
                    if (c == ' ') { innote = true; }
                    else if (c == '"') { cmd = formCmd._Name; inquote = true; }
                    else { cmd = formCmd.Name; cache_cmd += c; }
                    break;
                case formCmd.Name://命名
                    switch (c)
                    {
                        case '\r':
                        case ' ':
                        case '\n':
                        case '\t':
                            break;
                        case '('://命名截止
                            cmd = formCmd.param;
                            param_cmd_c = new Dictionary<string, object>();
                            break;
                        default:
                            cache_cmd += c;
                            break;
                    }
                    break;
                case formCmd._Name://引号命名
                    if (inchange)//转义模式
                    {
                        switch (c)
                        {
                            case '"':
                            case '\\':
                                cache += c;
                                break;
                            case 'n':
                                cache += '\n';
                                break;
                            case 't':
                                cache += '\t';
                                break;
                            default:
                                break;
                        }
                        inchange = false;
                    }
                    else//非转义模式
                    {
                        switch (c)
                        {
                            case '"'://结束命名
                                cell_c = new StatusCell(cache);//创建单元对象
                                cache = "";//清空缓存
                                cmd = formCmd.Nameok;
                                inquote = false;
                                break;
                            case '\\':
                                inchange = true;
                                break;
                            default:
                                cache += c;
                                break;
                        }
                    }
                    break;
                case formCmd.Nameok:
                    switch (c)
                    {
                        case '(':
                            cmd = formCmd.param;
                            param_cmd_c = new Dictionary<string, object>();
                            break;
                        default:

                            cmd_erro("指令格式出错",c);
                            break;

                    }
                    break;
                case formCmd.param:
                    if (!Parse_Param(c))//参数解析错误
                    {
                        cmd_erro("指令参数解析错误", c);
                        return false;
                    }
                    break;
                case formCmd.paramok://参数封装完毕
                    switch (cache_cmd)
                    {
                        case "import":
                            cmd_import(param_cmd_c);
                            break;
                        case "pack":
                            cmd_pack(param_cmd_c);
                            break;
                        case "up":
                            cmd_up(param_cmd_c);
                            break;

                    }
                    cmd = formCmd.non;
                    break;

            }
            return true;
        }

        private bool Parse_Cell(char c)//解析单元
        {
            switch (cell)
            {
                case formCell.waitCname://等待单元命名
                    switch (c)
                    {
                        case ' '://空白字符跳过
                        case '\t':
                        case '\n':
                        case ';':
                        case '\r':
                            break;
                        case '"'://转为引用模式
                            inquote = true;
                            cell = formCell.Cname;
                            break;
                        default:
                            cache += c;
                            cell = formCell.Cname;
                            break;
                    }
                    break;
                case formCell.Cname://单元命名
                    if (inquote)//引用模式
                    {
                        if (inchange)//转义模式
                        {
                            switch (c)
                            {
                                case '"':
                                case '\\':
                                    cache += c;
                                    break;
                                case 'n':
                                    cache += '\n';
                                    break;
                                case 't':
                                    cache += '\t';
                                    break;
                                default:
                                    break;
                            }
                            inchange = false;
                        }
                        else//非转义模式
                        {
                            switch (c)
                            {
                                case '"'://结束命名
                                    cell_c = new StatusCell(cache);//创建单元对象
                                    cache = "";//清空缓存
                                    cell = formCell.Cnameok;
                                    inquote = false;
                                    break;
                                case '\\':
                                    inchange = true;
                                    break;
                                default:
                                    cache += c;
                                    break;
                            }
                        }
                    }
                    else//非引用模式
                    {
                        switch (c)
                        {
                            case ' ':
                            case '\t':
                            case '\n':
                            case '\r':
                                break;
                            case '{'://命名结束
                                cell_c = new StatusCell(cache);
                                cache = "";
                                cell = formCell.stepok;
                                step = formStep.waitSname;
                                break;
                            default:
                                cache += c;
                                break;
                        }
                    }
                    break;
                case formCell.Cnameok://引用命名完毕
                    switch (c)
                    {
                        case ' ':
                        case '\t':
                        case '\r':
                        case '\n': break;
                        case '{':
                            cell = formCell.stepok;
                            step = formStep.waitSname;
                            break;
                        default:
                            cmd_erro("单元解析异常",c);
                            return false;
                    }
                    break;
                case formCell.step://步骤编辑
                    if (!Parse_Step(c))//解析出错则抛出错误信息并终止（生成步骤信息，完成后将步骤信息封装进单元内）
                    {
                        return false;
                    }
                    break;
                case formCell.stepok://步骤编辑完毕
                    switch (c)
                    {
                        case ' ':
                        case '\t':
                        case '\r':
                        case ';':
                        case '\n': break;
                        case '}'://单元完整编辑结构（将单元封装进库内）
                            sm.Add(cell_c);
                            cell_c = null;
                            cell = formCell.waitCname;
                            break;
                        default://说明还有未添加的步骤
                            cell = formCell.step;
                            step = formStep.waitSname;
                            if (!Parse_Step(c))//解析出错则抛出错误信息并终止（生成步骤信息，完成后将步骤信息封装进单元内）
                            {
                                return false;
                            }
                            break;
                    }
                    break;
            }
            return true;
        }

        private bool Parse_Step(char c)
        {
            switch (step)
            {
                case formStep.waitSname://等待步骤命名
                    switch (c)
                    {
                        case ' '://空白字符跳过
                        case '\t':
                        case '\n':
                        case ';':
                        case '\r':
                            break;
                        case '"'://转为引用模式
                            inquote = true;
                            step = formStep.Sname;
                            break;
                        default:
                            cache += c;
                            step = formStep.Sname;
                            break;
                    }
                    break;
                case formStep.Sname://步骤命名
                    if (inquote)//引用模式
                    {
                        if (inchange)//转义模式
                        {
                            switch (c)
                            {
                                case '"':
                                case '\\':
                                    cache += c;
                                    break;
                                case 'n':
                                    cache += '\n';
                                    break;
                                case 't':
                                    cache += '\t';
                                    break;
                                default:
                                    break;
                            }
                            inchange = false;
                        }
                        else//非转义模式
                        {
                            switch (c)
                            {
                                case '"'://结束命名
                                    step_c = new StepInfo();//生成步骤缓存
                                    function_c = new FunctionInfo() { name = cache };//生成步骤中的函数缓存
                                    cache = "";//清空缓存
                                    step = formStep.Snameok;
                                    inquote = false;
                                    break;
                                case '\\':
                                    inchange = true;
                                    break;
                                default:
                                    cache += c;
                                    break;
                            }
                        }
                    }
                    else//非引用模式
                    {
                        switch (c)
                        {
                            case ' ':
                            case '\t':
                            case '\n':
                                break;
                            case '('://命名结束
                                step_c = new StepInfo();//生成步骤缓存
                                function_c = new FunctionInfo() { name = cache };//生成步骤中的函数缓存
                                cache = "";//清空缓存
                                step = formStep.param;
                                param = formParam.waitKname;
                                param_c = new Dictionary<string, object>();//生成参数缓存
                                break;
                            default:
                                cache += c;
                                break;
                        }
                    }
                    break;
                case formStep.Snameok://引用命名完毕
                    switch (c)
                    {
                        case ' ':
                        case '\t':
                        case '\r':
                        case '\n': break;
                        case '(':
                            step = formStep.param;
                            param = formParam.waitKname;
                            param_c = new Dictionary<string, object>();//生成参数缓存
                            break;
                        default:
                            cmd_erro("步骤解析异常:参数结束后应为\":\"或者\"｛...｝\"", c);
                            return false;
                    }
                    break;
                case formStep.param://参数
                    if (!Parse_Param(c))//解析出错则抛出错误信息并终止（生成参数信息，完成后将参数信息封装进函数内,将函数封装进步骤内）
                    {
                        cmd_erro("参数解析异常",c);
                        return false;
                    }
                    break;
                case formStep.paramok://参数处理完毕
                    switch (c)
                    {
                        case ' ':
                        case '\t':
                        case '\r':
                        case '\n': break;
                        case '{'://出口开始标记
                            step = formStep.took;
                            break;
                        case ';'://出口结束标记(步骤封装完毕)
                            cell_c.AddStep(step_c);
                            cell = formCell.stepok;
                            break;
                        default:
                            cmd_erro("步骤解析异常:参数结束后应为\":\"或者\"｛...｝\"", c);
                            return false;
                    }
                    break;
                case formStep.waitTo://等待出口
                    switch (c)
                    {
                        case ' ':
                        case '\t':
                        case '\r':
                        case '\n': break;
                        case '{'://开启出口封装
                            step = formStep.took;
                            break;
                        case ';'://步骤封装完毕
                            cell_c.AddStep(step_c);
                            cell = formCell.stepok;
                            break;
                    }
                    break;
                case formStep.to://出口
                    if (!Parse_To(c))//解析出错则抛出错误信息并终止(生成出口信息，完成后并将出口信息封装进步骤内)
                    {
                        cmd_erro("出口解析异常", c);
                        return false;
                    }
                    break;
                case formStep.took://出口处理完毕(整个步骤解析完毕)
                    switch (c)
                    {
                        case ' ':
                        case '\t':
                        case '\r':
                        case ';':
                        case '\n': break;
                        case '}'://步骤的完整结构（步骤解析完毕，将步骤封装进单元内）
                            cell_c.AddStep(step_c);
                            cell = formCell.stepok;
                            break;
                        default://说明还有出口未封装
                            to = formTo.waitTname;
                            step = formStep.to;
                            if (!Parse_To(c))//解析出错则抛出错误信息并终止(生成出口信息，完成后并将出口信息封装进步骤内)
                            {
                                cmd_erro("出口解析异常", c);
                                return false;
                            }
                            break;
                    }
                    break;
            }
            return true;
        }

        private bool Parse_Param(char c)
        {
            switch (param)
            {
                case formParam.waitKname://等待键命名
                    switch (c)
                    {
                        case ' '://空白字符跳过
                        case '\t':
                        case '\n':
                        case '\r':
                            break;
                        case ')':
                            #region 跳转状态
                            //判断当前处于哪个状态
                            if (cmd != formCmd.non)//处于指令解析部分
                            {
                                cmd = formCmd.paramok;
                                Parse_cmd(' ');
                                break;
                            }
                            else if (step == formStep.param)//处于步骤头参数部分
                            {
                                //此时步骤的参数部分封装完毕
                                step = formStep.paramok;
                                //此时函数部分封装完毕
                                step_c.function = function_c;
                            }
                            else if (step == formStep.to)//处于步骤出口函数参数部分
                            {
                                //此时函数参数封装完毕
                                func = formFunc.paramok;
                            }
                            function_c.parameters = param_c;
                            param_c = null;//清空缓存
                            #endregion
                            break;
                        case '"'://转为引用模式
                            inquote = true;
                            param = formParam.Kname;
                            break;
                        default:
                            cache += c;
                            param = formParam.Kname;
                            break;
                    }
                    break;
                case formParam.Kname://键命名
                    if (inquote)//引用模式
                    {
                        if (inchange)//转义模式
                        {
                            switch (c)
                            {
                                case '"':
                                case '\\':
                                    cache += c;
                                    break;
                                case 'n':
                                    cache += '\n';
                                    break;
                                case 't':
                                    cache += '\t';
                                    break;
                                default:
                                    break;
                            }
                            inchange = false;
                        }
                        else//非转义模式
                        {
                            switch (c)
                            {
                                case '"'://结束命名
                                    //暂时不清除缓存，因为匹配键值对时要用
                                    param = formParam.Knameok;
                                    inquote = false;
                                    break;
                                case '\\':
                                    inchange = true;
                                    break;
                                default:
                                    cache += c;
                                    break;
                            }
                        }
                    }
                    else//非引用模式
                    {
                        switch (c)
                        {
                            case ' ':
                            case '\t':
                            case '\n':
                            case '\r':
                                break;
                            case ':'://命名结束
                                //暂时不清除缓存，因为匹配键值对时要用
                                param = formParam.waitVname;
                                break;
                            default:
                                cache += c;
                                break;
                        }
                    }
                    break;
                case formParam.Knameok://键完成命名
                    switch (c)
                    {
                        case ':':
                            param = formParam.waitVname;
                            break;
                        default:
                            cmd_erro("键名解析异常", c);
                            return false;
                    }
                    break;
                case formParam.waitVname://等待值命名
                    switch (c)
                    {
                        case ' '://空白字符跳过
                        case '\t':
                        case '\n':
                        case '\r':
                            break;
                        case '"'://转为引用模式
                            inquote = true;
                            param = formParam.Vname;
                            break;
                        default:
                            cache2 += c;
                            param = formParam.Vname;
                            break;
                    }
                    break;
                case formParam.Vname://值命名使用cache2
                    if (inquote)//引用模式
                    {
                        if (inchange)//转义模式
                        {
                            switch (c)
                            {
                                case '"':
                                case '\\':
                                    cache2 += c;
                                    break;
                                case 'n':
                                    cache2 += '\n';
                                    break;
                                case 't':
                                    cache2 += '\t';
                                    break;
                                default:
                                    break;
                            }
                            inchange = false;
                        }
                        else//非转义模式
                        {
                            switch (c)
                            {
                                case '"'://结束命名
                                    if (cmd == formCmd.non)
                                    {
                                        param_c.Add(cache, Changevalue(cache2));//生成键值对
                                    }
                                    else
                                    {
                                        param_cmd_c.Add(cache, Changevalue(cache2));//生成键值对
                                    }
                                    cache = "";//清空缓存
                                    cache2 = "";//清空缓存
                                    param = formParam.Vnameok;
                                    inquote = false;
                                    break;
                                case '\\':
                                    inchange = true;
                                    break;
                                default:
                                    cache2 += c;
                                    break;
                            }
                        }
                    }
                    else//非引用模式
                    {
                        switch (c)
                        {
                            case ' ':
                            case '\t':
                            case '\r':
                            case '\n':
                                break;
                            case ','://命名结束并且继续
                                if (cmd == formCmd.non)
                                {
                                    param_c.Add(cache, Changevalue(cache2));//生成键值对
                                }
                                else
                                {
                                    param_cmd_c.Add(cache, Changevalue(cache2));//生成键值对
                                }

                                cache = "";//清空缓存
                                cache2 = "";//清空缓存
                                param = formParam.waitKname;
                                break;
                            case ')'://命名结束，且参数封装完毕
                                if (cmd == formCmd.non)
                                {
                                    param_c.Add(cache, Changevalue(cache2));//生成键值对
                                }
                                else
                                {
                                    param_cmd_c.Add(cache, Changevalue(cache2));//生成键值对
                                }
                                cache = "";//清空缓存
                                cache2 = "";//清空缓存
                                param = formParam.waitKname;
                                #region 跳转状态
                                //判断当前处于哪个状态
                                if (cmd != formCmd.non)//处于指令解析部分
                                {
                                    cmd = formCmd.paramok;
                                    Parse_cmd(' ');
                                    break;
                                }
                                else if (step == formStep.param)//处于步骤头参数部分
                                {
                                    //此时步骤的参数部分封装完毕
                                    step = formStep.paramok;
                                    //此时函数部分封装完毕
                                    step_c.function = function_c;
                                }
                                else if (step == formStep.to)//处于步骤出口函数参数部分
                                {
                                    //此时函数参数封装完毕
                                    func = formFunc.paramok;
                                }
                                function_c.parameters = param_c;
                                param_c = null;//清空缓存
                                #endregion
                                break;
                            default:
                                cache2 += c;
                                break;
                        }
                    }
                    break;
                case formParam.Vnameok://值完成命名
                    switch (c)
                    {
                        case ' '://空白字符跳过
                        case '\t':
                        case '\r':
                        case '\n':
                            break;
                        case ','://获取键值对继续
                            param = formParam.waitKname;
                            break;
                        case ')':
                            #region 跳转状态
                            //判断当前处于哪个状态
                            if (cmd != formCmd.non)//处于指令解析部分
                            {
                                cmd = formCmd.paramok;
                                Parse_cmd(' ');
                                break;
                            }
                            else if (step == formStep.param)//处于步骤头参数部分
                            {
                                //此时步骤的参数部分封装完毕
                                step = formStep.paramok;
                                //此时函数部分封装完毕
                                step_c.function = function_c;
                            }
                            else if (step == formStep.to)//处于步骤出口函数参数部分
                            {
                                //此时函数参数封装完毕
                                func = formFunc.paramok;
                            }
                            function_c.parameters = param_c;
                            param_c = null;//清空缓存
                            #endregion
                            break;
                        default:
                            cmd_erro("键值对解析异常", c);
                            break;
                    }
                    break;
            }
            return true;
        }

        private bool Parse_To(char c)
        {
            switch (to)
            {
                case formTo.waitTname://等待出口命名
                    switch (c)
                    {
                        case ' '://空白字符跳过
                        case '\t':
                        case '\n':
                        case ';':
                        case '\r':
                            break;
                        case '"'://转为引用模式
                            inquote = true;
                            to = formTo.Tname;
                            break;
                        default:
                            cache += c;
                            to = formTo.Tname;
                            break;
                    }
                    break;
                case formTo.Tname://出口命名
                    if (inquote)//引用模式
                    {
                        if (inchange)//转义模式
                        {
                            switch (c)
                            {
                                case '"':
                                case '\\':
                                    cache += c;
                                    break;
                                case 'n':
                                    cache += '\n';
                                    break;
                                case 't':
                                    cache += '\t';
                                    break;
                                default:
                                    break;
                            }
                            inchange = false;
                        }
                        else//非转义模式
                        {
                            switch (c)
                            {
                                case '"'://结束命名
                                    skip_c = new SkipInfo() { name = cache };//生成出口对象
                                    cache = "";//清空命名缓存
                                    to = formTo.Tnameok;
                                    inquote = false;
                                    break;
                                case '\\':
                                    inchange = true;
                                    break;
                                default:
                                    cache += c;
                                    break;
                            }
                        }
                    }
                    else//非引用模式
                    {
                        switch (c)
                        {
                            case ' ':
                            case '\t':
                            case '\r':
                            case '\n':
                                break;
                            case '('://命名结束
                                skip_c = new SkipInfo() { name = cache };//生成出口对象
                                cache = "";//清空命名缓存
                                to = formTo.waitIname;
                                break;
                            default:
                                cache += c;
                                break;
                        }
                    }
                    break;
                case formTo.Tnameok://出口完成命名
                    switch (c)
                    {
                        case ' ':
                        case '\t':
                        case '\n':
                        case '\r':
                            break;
                        case '('://命名结束
                            to = formTo.waitIname;
                            break;
                        default:
                            cmd_erro("出口名称解析错误", c);
                            return false;
                    }
                    break;
                case formTo.waitIname://等待条件命名
                    switch (c)
                    {
                        case ' '://空白字符跳过
                        case '\t':
                        case '\n':
                        case '\r':
                            break;
                        case '"'://转为引用模式
                            inquote = true;
                            to = formTo.Iname;
                            break;
                        default:
                            cache += c;
                            to = formTo.Iname;
                            break;
                    }
                    break;
                case formTo.Iname://条件命名
                    if (inquote)//引用模式
                    {
                        if (inchange)//转义模式
                        {
                            switch (c)
                            {
                                case '"':
                                case '\\':
                                    cache += c;
                                    break;
                                case 'n':
                                    cache += '\n';
                                    break;
                                case 't':
                                    cache += '\t';
                                    break;
                                default:
                                    break;
                            }
                            inchange = false;
                        }
                        else//非转义模式
                        {
                            switch (c)
                            {
                                case '"'://结束命名
                                    skip_c.condition = cache;//设置条件
                                    cache = "";//清空命名缓存
                                    to = formTo.Inameok;
                                    inquote = false;
                                    break;
                                case '\\':
                                    inchange = true;
                                    break;
                                default:
                                    cache += c;
                                    break;
                            }
                        }
                    }
                    else//非引用模式
                    {
                        switch (c)
                        {
                            case ' ':
                            case '\t':
                            case '\n':
                            case '\r':
                                break;
                            case ','://命名结束
                                skip_c.condition = cache;//设置条件
                                cache = "";//清空命名缓存
                                to = formTo.waitIndex;
                                break;
                            default:
                                cache += c;
                                break;
                        }
                    }
                    break;
                case formTo.Inameok://条件命名完毕
                    switch (c)
                    {
                        case ' ':
                        case '\t':
                        case '\n':
                        case '\r':
                            break;
                        case ','://命名结束
                            to = formTo.waitIndex;
                            break;
                        default:
                            cmd_erro("出口条件解析错误", c);
                            return false;
                    }
                    break;
                case formTo.waitIndex://等待索引命名
                    switch (c)
                    {
                        case ' ':
                        case '\t':
                        case '\n':
                        case '\r':
                            break;
                        case '0':
                        case '1':
                        case '2':
                        case '3':
                        case '4':
                        case '5':
                        case '6':
                        case '7':
                        case '8':
                        case '9':
                            cache += c;
                            to = formTo.Index;
                            break;
                        default:
                            cmd_erro("出口索引解析出错", c);
                            return false;
                    }
                    break;
                case formTo.Index://索引命名
                    switch (c)
                    {
                        case ' ':
                        case '\t':
                        case '\n':
                        case '\r':
                            break;
                        case '0':
                        case '1':
                        case '2':
                        case '3':
                        case '4':
                        case '5':
                        case '6':
                        case '7':
                        case '8':
                        case '9':
                            cache += c;
                            to = formTo.Index;
                            break;
                        case ')'://命名结束
                            if (Regex.IsMatch(cache, @"^(0|[1-9][0-9]*)$"))
                            {
                                skip_c.index = Convert.ToInt32(cache);
                                cache = "";
                                to = formTo.waitFunction;
                            }
                            else
                            {
                                cmd_erro("出口索引解析出错", c);
                                return false;
                            }
                            break;
                        default:
                            cmd_erro("出口索引解析出错", c);
                            return false;
                    }
                    break;
                case formTo.waitFunction://等待函数封装
                    switch (c)
                    {
                        case ' ':
                        case '\t':
                        case '\r':
                        case '\n': break;
                        case '{'://函数封装起始符
                            to = formTo.functionok;
                            break;
                        case ';'://出口封装完毕
                            step_c.skips.Add(skip_c);
                            skip_c = null;
                            step = formStep.took;
                            break;
                        default:
                            cmd_erro("出口解析错误", c);
                            return false;
                    }
                    break;
                case formTo.function://生成函数信息，完成后并将函数信息封装进出口内
                    if (!Parse_Func(c))//生成函数信息，完成后封装进出口
                    {
                        cmd_erro("出口函数解析错误", c);
                        return false;
                    }
                    break;
                case formTo.functionok://一个完整函数封装完后
                    switch (c)
                    {
                        case ' ':
                        case '\t':
                        case '\n':
                        case '\r':
                            break;
                        case '}'://出口封装结束
                            step_c.skips.Add(skip_c);
                            skip_c = null;
                            step = formStep.took;
                            break;
                        default://还有未封装的函数
                            func = formFunc.waitFname;
                            to = formTo.function;
                            if (!Parse_Func(c))//生成函数信息，完成后封装进出口
                            {
                                cmd_erro("出口函数解析错误", c);
                                return false;
                            }
                            break;
                    }
                    break;
            }
            return true;
        }

        private bool Parse_Func(char c)
        {
            switch (func)
            {
                case formFunc.waitFname://等待函数命名
                    switch (c)
                    {
                        case ' '://空白字符跳过
                        case '\t':
                        case '\n':
                        case '\r':
                            break;
                        case '"'://转为引用模式
                            inquote = true;
                            func = formFunc.Fname;
                            break;
                        default:
                            cache += c;
                            func = formFunc.Fname;
                            break;
                    }
                    break;
                case formFunc.Fname://函数命名
                    if (inquote)//引用模式
                    {
                        if (inchange)//转义模式
                        {
                            switch (c)
                            {
                                case '"':
                                case '\\':
                                    cache += c;
                                    break;
                                case 'n':
                                    cache += '\n';
                                    break;
                                case 't':
                                    cache += '\t';
                                    break;
                                default:
                                    break;
                            }
                            inchange = false;
                        }
                        else//非转义模式
                        {
                            switch (c)
                            {
                                case '"'://结束命名
                                    function_c = new FunctionInfo() { name = cache };//生成函数信息
                                    cache = "";//清空命名缓存
                                    func = formFunc.Fnameok;
                                    inquote = false;
                                    break;
                                case '\\':
                                    inchange = true;
                                    break;
                                default:
                                    cache += c;
                                    break;
                            }
                        }
                    }
                    else//非引用模式
                    {
                        switch (c)
                        {
                            case ' ':
                            case '\t':
                            case '\n':
                            case '\r':
                                break;
                            case '('://命名结束
                                function_c = new FunctionInfo() { name = cache };//生成函数信息
                                cache = "";//清空命名缓存
                                func = formFunc.param;
                                param = formParam.waitKname;
                                param_c = new Dictionary<string, object>();//生成参数缓存
                                break;
                            default:
                                cache += c;
                                break;
                        }
                    }
                    break;
                case formFunc.Fnameok://函数命名完成
                    switch (c)
                    {
                        case ' ':
                        case '\t':
                        case '\r':
                        case '\n': break;
                        case '('://参数封装起始符
                            func = formFunc.param;
                            param = formParam.waitKname;
                            param_c = new Dictionary<string, object>();//生成参数缓存
                            break;
                        default:
                            cmd_erro("函数名称解释错误", c);
                            return false;
                    }
                    break;
                case formFunc.param://参数封装
                    if (!Parse_Param(c))
                    {
                        cmd_erro("函数参数解释错误", c);
                        return false;
                    }
                    break;
                case formFunc.paramok://参数封装完毕，
                    switch (c)
                    {
                        case ' ':
                        case '\t':
                        case '\r':
                        case '\n': break;
                        case ';'://此函数封装完毕
                            skip_c.functions.Add(function_c);
                            function_c = null;
                            to = formTo.functionok;
                            break;
                        default:
                            cmd_erro("函数封装错误", c);
                            return false;
                    }
                    break;
            }
            return true;
        }
    }
}
