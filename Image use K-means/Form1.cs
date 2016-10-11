using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Imaging;
using System.Threading;

namespace Image_use_K_means
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private class Site
        {
            public int X;
            public int Y;
            public int R, G, B;
        }
        static int Image_Height, Image_Width;
        static List<List<Site>> Cluster;
        static int[,,] Picture_Pixel, Result_Pixel;
        static int K = 3;
        static Bitmap Source, Result;
        private void button1_Click(object sender, EventArgs e)
        {
            
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                StreamReader file = new StreamReader(openFileDialog1.FileName);
                Source = new Bitmap(openFileDialog1.FileName);
                SourceShow.Image = Source;
                Image_Height = Source.Height;
                Image_Width = Source.Width;
                Cluster = Get_Initial_RandomSite(Image_Height, Image_Width);
                Picture_Pixel = GetRGBData(Source);
                Result_Pixel = GetRGBData(Source);
                KmeansFunction(Picture_Pixel, ResultShow);
                
            }


        }
        public static int[,,] GetRGBData(Bitmap bitImg)
        {
            int height = bitImg.Height;
            int width = bitImg.Width;
            //locking
            BitmapData bitmapData = bitImg.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            // get the starting memory place
            IntPtr imgPtr = bitmapData.Scan0;
            //scan width
            int stride = bitmapData.Stride;
            //scan ectual
            int widthByte = width * 3;
            // the byte num of padding
            int skipByte = stride - widthByte;
            //set the place to save values
            int[,,] rgbData = new int[height, width, 3];
            #region
            unsafe//專案－＞屬性－＞建置－＞容許Unsafe程式碼須選取。
            {
                byte* p = (byte*)(void*)imgPtr;
                for (int j = 0; j < height; j++)
                {
                    for (int i = 0; i < width; i++)
                    {
                        //B channel
                        rgbData[j, i, 2] = p[0];
                        p++;
                        //g channel
                        rgbData[j, i, 1] = p[0];
                        p++;
                        //R channel
                        rgbData[j, i, 0] = p[0];
                        p++;
                    }
                    p += skipByte;
                }
            }
            bitImg.UnlockBits(bitmapData);
            #endregion
            return rgbData;
        }

        //K-means 
        public static void KmeansFunction(int[,,] Picture_Pixel,PictureBox re)
        {
            bool Change_Status = false;
            do
            {
                Classification();
                ReColoration(Result_Pixel);

                Change_Status = Reflash_Center_Site();
                ReColoration(Picture_Pixel);


                Result = Source.Clone(new Rectangle(0, 0, Image_Width, Image_Height), Source.PixelFormat);
                Result = SetRGBData(Result_Pixel);
                re.Image = Result;
                re.Update();
                Thread.Sleep(800);
            } while (!Change_Status);
            
            
        }
        private static double Compute_Distance( Site Site_One, Site Site_Two)
        {
            if(Site_One.X == -1 && Site_One.Y == -1 )
                return (Math.Pow((Site_One.R - Picture_Pixel[Site_Two.X, Site_Two.Y, 0]), 2) +
            Math.Pow((Site_One.G - Picture_Pixel[Site_Two.X, Site_Two.Y, 1]), 2) +
            Math.Pow((Site_One.B - Picture_Pixel[Site_Two.X, Site_Two.Y, 2]), 2));

            return (Math.Pow((Picture_Pixel[Site_One.X, Site_One.Y, 0] - Picture_Pixel[Site_Two.X, Site_Two.Y, 0]), 2) +
            Math.Pow((Picture_Pixel[Site_One.X, Site_One.Y, 1] - Picture_Pixel[Site_Two.X, Site_Two.Y, 1]), 2) +
            Math.Pow((Picture_Pixel[Site_One.X, Site_One.Y, 2] - Picture_Pixel[Site_Two.X, Site_Two.Y, 2]), 2));
        }
        private static bool Check_SameSite(List<List<Site>> Culster, Site Target)
        {
            if (Culster.Count == 0)
                return true;
            bool Status = true;
            foreach (var Each in Culster)
                if (Each[0].X == Target.X)
                    if (Each[0].Y == Target.Y)
                        Status = false;
            return Status;
        }
        private static List<List<Site>> Get_Initial_RandomSite(int Height, int Width)
        {
            Random Ran = new Random(Guid.NewGuid().GetHashCode());
            List<List<Site>> Culster = new List<List<Site>>();
            do
            {
                List<Site> Random_List = new List<Site>();
                Site Temp_Site = new Site();
                Temp_Site.X = Ran.Next(0, Height);
                Temp_Site.Y = Ran.Next(0, Width);
                if (Check_SameSite(Culster, Temp_Site))
                    Random_List.Add(Temp_Site);
                else
                    continue;
                Culster.Add(Random_List);
            } while (Culster.Count < K);
            return Culster;
        }
        private static void Classification()
        {
            for (int Index_Hieght = 0; Index_Hieght < Image_Height; Index_Hieght++)
            {
                for (int Index_Width = 0; Index_Width < Image_Width; Index_Width++)
                {
                    Site This_Site = new Site() { X = Index_Hieght, Y = Index_Width };
                    int Shortest_DistanceNum = 0;
                    double Shortest_Distance = Compute_Distance(Cluster[0][0], This_Site);

                    for (int Index_KNum = 1; Index_KNum < K; Index_KNum++)
                    {
                        var New_Distance = Compute_Distance(Cluster[Index_KNum][0], This_Site);
                        if (New_Distance < Shortest_Distance)
                        {
                            Shortest_Distance = New_Distance;
                            Shortest_DistanceNum = Index_KNum;
                        }
                    }
                    Cluster[Shortest_DistanceNum].Add(This_Site);
                }
            }
        }

        private static bool Reflash_Center_Site()
        {
            bool Over_Status=true;
            for(int Index = 0; Index < K; Index++)
            {
                var New_Center = Find_Center_Site(Cluster[Index]);
                if(!Same_Center(Cluster[Index][0], New_Center))
                {
                    Cluster[Index].Clear();
                    Cluster[Index].Add(New_Center);
                    Over_Status = false;
                }
            }
            return Over_Status;
        }
        private static Site Find_Center_Site(List<Site> One_Cluster)
        {
            if (One_Cluster.Count == 1)
                return One_Cluster[0];
            int Cluster_Num = One_Cluster.Count;
            int Total_R = 0, Total_G = 0, Total_B = 0;
            for (int index = 1; index < Cluster_Num; index++)
            {
                Total_R += Picture_Pixel[One_Cluster[index].X, One_Cluster[index].Y, 0];
                Total_G += Picture_Pixel[One_Cluster[index].X, One_Cluster[index].Y, 1];
                Total_B += Picture_Pixel[One_Cluster[index].X, One_Cluster[index].Y, 2];
            }
            Total_R /= Cluster_Num - 1;
            Total_G /= Cluster_Num - 1;
            Total_B /= Cluster_Num - 1;
            //int[] Center_Pixel = new int[3] { Total_R, Total_G, Total_B };


            Site Center_Site = new Site { X=-1,Y=-1, R = Total_R , G = Total_G , B = Total_B };
            //int Center_Distance = Count_Pixel_Distance( Center_Site, Center_Pixel);
            //for (int index = 2; index < Cluster_Num; index++)
            //{
            //    int New_Distance = Count_Pixel_Distance( One_Cluster[index], Center_Pixel);
            //    if (New_Distance < Center_Distance)
            //    {
            //        Center_Distance = New_Distance;
            //        Center_Site = One_Cluster[index];
            //    }
            //}

            return Center_Site;
        }
        private static bool Same_Center(Site Site_One , Site Site_Two)
        {
            if (Site_One.R != Site_Two.R)
                return false;
            if (Site_One.G != Site_Two.G)
                return false;
            if (Site_One.B != Site_Two.B)
                return false;
            return true;
        }
        private static int Count_Pixel_Distance(Site Picture_Pixel_Site,int[] Center)
        {
            return (Math.Abs(Picture_Pixel[Picture_Pixel_Site.X, Picture_Pixel_Site.Y, 0] - Center[0]) +
                Math.Abs(Picture_Pixel[Picture_Pixel_Site.X, Picture_Pixel_Site.Y, 1] - Center[1]) +
                Math.Abs(Picture_Pixel[Picture_Pixel_Site.X, Picture_Pixel_Site.Y, 2] - Center[2]));
        }

        private static void ReColoration(int[,,]Target_Pixel)
        {
            foreach(var Each_Cluster in Cluster)
            {
                int This_Cluster_Count = Each_Cluster.Count;
                for (int Cluster_Index = 1; Cluster_Index < This_Cluster_Count; Cluster_Index++)
                {
                    Target_Pixel[Each_Cluster[Cluster_Index].X, Each_Cluster[Cluster_Index].Y, 0] = Each_Cluster[0].R;
                    Target_Pixel[Each_Cluster[Cluster_Index].X, Each_Cluster[Cluster_Index].Y, 1] = Each_Cluster[0].G;
                    Target_Pixel[Each_Cluster[Cluster_Index].X, Each_Cluster[Cluster_Index].Y, 2] = Each_Cluster[0].B;
                }
            }
        }
        public static Bitmap SetRGBData(int[,,] rgbData)
        {
            //宣告Bitmap變數
            Bitmap bitImg;
            int width = rgbData.GetLength(1);
            int height = rgbData.GetLength(0);

            //依陣列長寬設定Bitmap新的物件
            bitImg = new Bitmap(width, height, PixelFormat.Format24bppRgb);

            //鎖住Bitmap整個影像內容
            BitmapData bitmapData = bitImg.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);
            //取得影像資料的起始位置
            IntPtr imgPtr = bitmapData.Scan0;
            //影像scan的寬度
            int stride = bitmapData.Stride;
            //影像陣列的實際寬度
            int widthByte = width * 3;
            //所Padding的Byte數
            int skipByte = stride - widthByte;

            #region 設定RGB資料
            //注意C#的GDI+內的影像資料順序為BGR, 非一般熟悉的順序RGB
            //因此我們把順序調回GDI+的設定值, RGB->BGR
            unsafe
            {
                byte* p = (byte*)(void*)imgPtr;
                for (int j = 0; j < height; j++)
                {
                    for (int i = 0; i < width; i++)
                    {
                        //B Channel
                        p[0] = (byte)rgbData[j, i, 2];
                        p++;
                        //G Channel
                        p[0] = (byte)rgbData[j,i, 1];
                        p++;
                        //B Channel
                        p[0] = (byte)rgbData[j, i, 0];
                        p++;
                    }
                    p += skipByte;
                }
            }

            //解開記憶體鎖
            bitImg.UnlockBits(bitmapData);

            #endregion

            return bitImg;
        }
    }
}
