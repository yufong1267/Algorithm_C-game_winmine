using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HW5_B10517007_歐育綸_V1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private Button[,] btn = new Button[20, 20];
        private int[,] point_table = new int[20, 20];
        private int count_triangle = 0;

        private void Form1_Load(object sender, EventArgs e)
        {
            //先做地雷跟分數表的初始化
            bool[,] table = new bool[20, 20];
            for(int i = 0; i < 20; i++)
            {
                for(int j = 0; j < 20; j++)
                {
                    table[i, j] = false;
                    point_table[i, j] = 0;
                }
            }
            //亂數生產哪40個點是地雷
            // bool check_first = false;
            string test = "";
            for(int i = 0; i < 40; i++)
            {
                Random rnd = new Random(Guid.NewGuid().GetHashCode());
                int loc_x = rnd.Next(0, 20);
                int loc_y = rnd.Next(0, 20);
                table[loc_x, loc_y] = true; 
            }
            
            //計算每個點的分數 如果是地雷就points9 不是就看幾個
            for (int i = 0; i  < 20; i++)
            {
                for(int j = 0; j < 20; j++)
                {
                    //先檢查是不是地雷
                    if(table[i,j])
                    {
                        point_table[i, j] = 9;
                        count_triangle++;
                    }
                    else
                    {
                        int count = 0;
                        if(i-1 >=0 )
                        {
                            if (table[i - 1, j])//上
                            {
                                count++;
                            }
                        }
                       
                        if(i+1 <= 19)
                        {
                            if (table[i + 1, j])//下
                            {
                                count++;
                            }
                        }

                        if (j - 1 >= 0)
                        {
                            if (table[i, j - 1])//左
                            {
                                count++;
                            }
                        }
                        
                        if(j+1<=19)
                        {
                            if (table[i, j + 1])//右
                            {
                                count++;
                            }
                        }
                       
                        if(i-1 >= 0 && j-1 >= 0)
                        {
                            if (table[i - 1, j - 1])//左上
                            {
                                count++;
                            }
                        }
                       
                        if(i-1 >= 0 && j+1 <= 19)
                        {
                            if (table[i - 1, j + 1])//右上
                            {
                                count++;
                            }
                        }
                       
                        if(i+1 <= 19 && j-1 >= 0)
                        {
                            if (table[i + 1, j - 1])//左下
                            {
                                count++;
                            }
                        }
                       
                        if(i+1 <= 19 && j+1 <= 19)
                        {
                            if (table[i + 1, j + 1])//右下
                            {
                                count++;
                            }
                        }
                        point_table[i, j] = count;
                    }
                }
            }

            //開始建立旗子
            int x = 30, y = 30;
            for(int i = 0; i < 20; i++)
            {
                x = 30;
                for(int j = 0; j <　20; j++)
                {
                    Button reg = new Button();
                    reg.Size = new Size(20, 20);
                    reg.Location = new Point(x,y);
                    reg.Text = " " + i + " " + j;
                    reg.Click += new EventHandler(btn_click);
                    reg.MouseDown += new MouseEventHandler(make_mark);

                    x += 20;
                    btn[i, j] = reg;
                    this.Controls.Add(btn[i, j]);
                }
                y += 20;
            }
        }
        

        private void make_mark(object sender, MouseEventArgs e)
        {
            //先偵測滑鼠右鍵
            if (e.Button == MouseButtons.Right)
            {
                //先獲取那個是哪個i j
                string x = (sender as Button).Text;
                
                string add_triangle = "▶";
                if (x.Contains(add_triangle))
                {
                    string g = " ";
                    (sender as Button).Text = g + x.Substring(1);
                }
                else
                {
                    (sender as Button).Text = add_triangle + x.Substring(1);
                }

                int reg_triangle = 0;
                //檢查每個marker點是不是 points9
                for (int i = 0; i < 20; i++)
                {
                    for (int j = 0; j < 20; j++)
                    {
                        if(btn[i,j].Text.Contains(add_triangle) && point_table[i,j] == 9)
                        {
                            reg_triangle++;
                        }
                    }
                }
                //檢查是不是跟一開始所檢查的數量一樣多
                if(reg_triangle >= count_triangle)
                {
                    MessageBox.Show("你終於贏了!!!!!!!!!");
                    Application.Restart();
                }
            }

        }

        private void btn_click(object sender, EventArgs e)
        {
            //先獲取那個是哪個i j
            string x = (sender as Button).Text;
            x = x.Substring(1);
            string[] temp = x.Split(' ');
            //string test = "";
            int r , i = 0 , j = 0;
            bool isI = true;
            foreach (string s in temp)
            {
                // MessageBox.Show("" + s);
                /*   if(!((s.Length == 1 || s.Length == 2) && Convert.ToChar(s) >= '0' && Convert.ToChar(s) <= '9'))
                    {
                        return;
                    }*/
                try
                {
                    r = Int32.Parse(s);
                    if (isI)
                    {
                        i = r;
                        isI = false;
                    }
                    else
                    {
                        j = r;
                        isI = true;
                    }
                }catch(Exception e2)
                {
                    return;
                }
                
            }
            if(point_table[i,j] == 9)
            {
                MessageBox.Show("Damn U lose Fucking ㄟ斯齁");
                Application.Restart();
            }
            (sender as Button).Text = "" + point_table[i, j];
            (sender as Button).Enabled = false;
           if(i-1 >= 0 && point_table[i, j] == 0)
            {
                btn_click(btn[i - 1, j] , null); //上
            }
            if (j - 1 >= 0 && point_table[i, j] == 0)
            {
                btn_click(btn[i , j - 1] , null); //左
            }
            if (i + 1 <= 19 && point_table[i, j] == 0)
            {
                btn_click(btn[i +1 , j], null); //下
            }
            if (j + 1 <= 19 && point_table[i, j] == 0)
            {
                btn_click(btn[i, j + 1], null); //右
            }

        }
        
    }
}
