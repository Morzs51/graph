using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;
using System.Text;

public class graph : MonoBehaviour
{

    // Start is called before the first frame update
    [SerializeField]
    LineRenderer line;
    [SerializeField]
    InputField input;
    [SerializeField]
    Canvas canv;

    // Update is called once per frame
    public void render()
    {
        GameObject myline = new GameObject();
        line.startWidth = 0.01f;
        line.endWidth = 0.01f;
        line.positionCount = 0;
        Vector3 start = new Vector3(0, 0);
        Vector3 end = new Vector3();
        float RatioX = canv.GetComponent<RectTransform>().transform.localScale.x;
        float RatioY = canv.GetComponent<RectTransform>().transform.localScale.y;
        line.positionCount++;
        line.SetPosition(0, start);
        int i = 0;
        for (float x=0;x-50<0.0001;x+=0.1f)
        {
            line.positionCount++;
            end.x = x;
            end.y = f(x);
            line.SetPosition(++i,end);
        }

    }


        public float f(float x)
        {
            //5-5/2*3-2*10
            //3+(7-5/(2+3)*(7-2))
            //(8*8)^(1/2)
            //sin(exp(6)/3-5)^2+cos(exp(6)/3-5)^2
            //2^2^2
            string func = input.text;
            int i = 0;
            Lexem item;
            Merge save;
            char ch = '0';
            double value = 0;
            bool flag = true;
            Stack<Lexem> n = new Stack<Lexem>();
            Stack<Lexem> op = new Stack<Lexem>();
            while (i < func.Length)
            {
                ch = func[i];
                while (ch == ' ')//игнор пробелов
                    ch = func[++i];
                if (ch == 's' || ch == 'c' || ch == 't' || ch == 'e' || ch == 'l')
                {
                    string f = Char.ToString(ch);
                    while (!prov(f))
                    {
                        f += Char.ToString(func[++i]);
                    }
                    switch (f)
                    {
                        case "cos":
                            item.value = 0;
                            item.type = "cos";
                            op.Push(item);
                            break;
                        case "sin":
                            item.value = 0;
                            item.type = "sin";
                            op.Push(item);
                            break;
                        case "tg":
                            item.value = 0;
                            item.type = "tg";
                            op.Push(item);
                            break;
                        case "ctg":
                            item.value = 0;
                            item.type = "ctg";
                            op.Push(item);
                            break;
                        case "exp":
                            item.value = 0;
                            item.type = "exp";
                            op.Push(item);
                            break;
                        case "ln":
                            item.value = 0;
                            item.type = "ln";
                            op.Push(item);
                            break;
                    }

                    i++;

                }
                if (ch == 'p' || ch == 'P' || ch == '-' && flag)
                {
                    if (ch == '-' && flag)
                    {
                        i++;
                        ch = func[i];
                    }
                    else flag = false;
                    item.type = "0";
                    item.value = Math.PI;
                    if (flag)
                    {
                        flag = false;
                        item.value *= -1;
                    }
                    n.Push(item);
                    i++;

                }
            if (ch == 'x' || ch == 'X' || ch == '-' && flag)
            {
                if (ch == '-' && flag)
                {
                    i++;
                    ch = func[i];
                }
                else flag = false;
                item.type = "0";
                item.value = x;
                if (flag)
                {
                    flag = false;
                    item.value *= -1;
                }
                n.Push(item);
                i++;

            }
            if (ch >= '0' && ch <= '9' || ch == '-' && flag)
                {
                    value = 0;
                    if (ch == '-' && flag)
                    {
                        i++;
                        ch = func[i];
                    }
                    else flag = false;
                    while ((func[i] >= '0' && func[i] <= '9' || func[i] == '.' || func[i] == ',') && (i < func.Length))
                    {

                        if (func[i] == '.' || func[i] == ',')//надо будет изменить что бы дробные числа считало
                        {
                            if (i + 1 < func.Length)
                                i++;
                            ch = func[i];
                        }
                        else
                        {
                            value = value * 10 + ch - '0';
                            if (i + 1 <= func.Length)
                                i++;
                            if (i != func.Length)
                                ch = func[i];
                            else break;
                        }
                    }
                    item.type = "0";
                    if (flag)
                    {
                        flag = false;
                        value *= -1;
                    }
                    item.value = value;
                    //Console.WriteLine("value");
                    //Console.WriteLine(value);
                    n.Push(item);
                    //Console.WriteLine(n.Count);
                }
                if (ch == '+' || ch == '-' && !flag || ch == '*' || ch == '/' || ch == '^')
                {
                    if (op.Count == 0)//если стек с операциями пуст 
                    {
                        item.type = Char.ToString(ch);
                        item.value = 0;
                        op.Push(item);
                    }
                    else
                    if (op.Count > 0 && getRang(Char.ToString(ch)) > getRang(op.Peek().type))//если стек с операциями не пуст но если приоритет текущей операции выше приоритета операции в стеке
                    {
                        item.type = Char.ToString(ch);
                        item.value = 0;
                        op.Push(item);
                    }
                    else
                    if (op.Count != 0 && getRang(Char.ToString(ch)) <= getRang(op.Peek().type))//если стек с операциями не пуст но если приоритет текущей операции ниже либо равен приоритета операции в стеке
                    {
                        save = S(n, op);
                        save = math(save);
                        item.type = Char.ToString(ch);
                        item.value = 0;
                        op.Push(item);
                    }
                    i++;
                }
                if (ch == '(')
                {
                    item.type = Char.ToString(ch);
                    item.value = 0;
                    op.Push(item);
                    i++;
                }
                if (ch == ')')
                {
                    save = S(n, op);
                    if (n.Count > 1)
                    {
                        while (save.op.Peek().type != "(")
                        {
                            save = math(save);
                            n = save.n;
                        }
                        // Console.WriteLine(op.Count);

                        save.op.Pop();
                    }
                    else
                    {
                        // Console.WriteLine(save.op.Peek().type);
                        save.op.Pop();
                    }
                    op = save.op;
                    n = save.n;
                    i++;
                }


            }
            //Console.WriteLine("svoboda");
            save = S(n, op);
            /*
            while(n.Count>0)
            {
               Console.WriteLine(n.Peek().value);
                n.Pop();
            }
            while (op.Count > 0)
            {
                Console.WriteLine(op.Peek().type);
                op.Pop();
            }
            */
            while (save.op.Count != 0)
            {
                save = math(save);
            }

        //Console.WriteLine("otvet");
        //
        //Console.WriteLine(save.n.Peek().value);
        //
        return (float)save.n.Peek().value;
        }
        public Merge math(Merge save)
        {
            Stack<Lexem> n = save.n;
            Stack<Lexem> op = save.op;
            double a = 0, b = 0, c = 0;
            Lexem item;
            bool prov = true;
            if (op.Peek().type != "(")
            {
                a = n.Peek().value;
                n.Pop();
            }

            switch (op.Peek().type)
            {
                case "+":
                    b = n.Peek().value;
                    n.Pop();
                    c = a + b;
                    item.value = c;
                    item.type = "0";
                    n.Push(item);
                    break;
                case "-":
                    b = n.Peek().value;
                    n.Pop();
                    c = b - a;
                    item.value = c;
                    item.type = "0";
                    n.Push(item);
                    break;
                case "*":
                    b = n.Peek().value;
                    n.Pop();
                    c = a * b;
                    item.value = c;
                    item.type = "0";
                    n.Push(item);
                    break;
                case "/":
                    //учесть потом что на 0 делить нельзя
                    b = n.Peek().value;
                    n.Pop();
                    c = b / a;
                    item.value = c;
                    item.type = "0";
                    n.Push(item);
                    break;
                case "^":
                    b = n.Peek().value;
                    n.Pop();
                    c = Math.Pow(b, a);
                    item.value = c;
                    item.type = "0";
                    n.Push(item);
                    break;
                case "sin":
                    c = Math.Sin(a);
                    if (Math.Abs(c) < 0.0000001)//костыли подъехали
                    {
                        c = 0;
                    }
                    item.value = c;
                    item.type = "0";
                    n.Push(item);
                    break;
                case "cos":
                    c = Math.Cos(a);
                    if (Math.Abs(c) < 0.0000001)//костыли подъехали
                    {
                        c = 0;
                    }
                    item.value = c;
                    item.type = "0";
                    n.Push(item);
                    break;
                case "tg":
                    c = Math.Tan(a);
                    if (Math.Abs(c) < 0.0000001)//костыли подъехали
                    {
                        c = 0;
                    }
                    item.value = c;
                    item.type = "0";
                    n.Push(item);
                    break;
                case "ctg":
                    c = Math.Cos(a) / Math.Sin(a);
                    if (Math.Abs(c) < 0.0000001)//костыли подъехали
                    {
                        c = 0;
                    }
                    item.value = c;
                    item.type = "0";
                    n.Push(item);
                    break;
                case "exp":
                    c = Math.Exp(a);
                    item.value = c;
                    item.type = "0";
                    n.Push(item);
                    break;
                case "ln":
                    c = Math.Log(a);
                    item.value = c;
                    item.type = "0";
                    n.Push(item);
                    break;
                default:
                    prov = false;
                    break;
            }
            if (prov)
            {
                //Console.WriteLine(a);
                // Console.WriteLine(b);
                op.Pop();
                //Console.WriteLine("c");
                //Console.WriteLine(c);
            }
            save = S(n, op);
            return save;
        }
        public Merge S(Stack<Lexem> n, Stack<Lexem> op)
        {
            Merge s;
            s.op = op;
            s.n = n;
            return s;
        }
        public int getRang(string ch)
        {
            if (ch == "-" || ch == "-")
                return 1;
            if (ch == "*" || ch == "/")
                return 2;
            if (ch == "^")
                return 3;
            if (prov(ch))
                return 4;
            else return 0;
        }
        public bool prov(string f)//проверка на название функции
        {
            switch (f)
            {
                case "cos":
                    return true;
                case "sin":
                    return true;
                case "tg":
                    return true;
                case "ctg":
                    return true;
                case "exp":
                    return true;
                case "ln":
                    return true;
            }
            return false;
        }
    }
    public struct Lexem
    {
        public string type;//для чисел 0 и далее соответствующий операции
        public double value;//
    }
    public struct Merge
    {
        public Stack<Lexem> n;
        public Stack<Lexem> op;
    }


