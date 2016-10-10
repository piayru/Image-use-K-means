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
        static int[,,] Picture_Pixel;
        static int K = 6;
        private void button1_Click(object sender, EventArgs e)
        {
            Bitmap Source, Result;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                StreamReader file = new StreamReader(openFileDialog1.FileName);
                Source = new Bitmap(openFileDialog1.FileName);
                SourceShow.Image = Source;
                Image_Height = Source.Height;
                Image_Width = Source.Width;
                Cluster = Get_Initial_RandomSite(Image_Height, Image_Width);
                Picture_Pixel = GetRGBData(Source);
                KmeansFunction(Picture_Pixel);
                
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
        public static void KmeansFunction(int[,,] Picture_Pixel)
        {
            bool Change_Status = false;
            do
            {
                Classification();
                Change_Status = Reflash_Center_Site();
                string site_1 = Cluster[0][0].X.ToString() + " , " + Cluster[0][0].Y.ToString();
                string site_2 = Cluster[1][0].X.ToString() + " , " + Cluster[1][0].Y.ToString();
                string site_3 = Cluster[2][0].X.ToString() + " , " + Cluster[2][0].Y.ToString();
                string site_4 = Cluster[3][0].X.ToString() + " , " + Cluster[3][0].Y.ToString();
                string site_5 = Cluster[4][0].X.ToString() + " , " + Cluster[4][0].Y.ToString();
                string site_6 = Cluster[5][0].X.ToString() + " , " + Cluster[5][0].Y.ToString();
            } while (!Change_Status);
        }
        private static double Compute_Distance( Site Site_One, Site Site_Two)
        {
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
                    double Shortest_Distance = Compute_Distance(This_Site, Cluster[0][0]);

                    for (int Index_KNum = 1; Index_KNum < K; Index_KNum++)
                    {
                        var New_Distance = Compute_Distance(This_Site, Cluster[Index_KNum][0]);
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
                if(Cluster[Index][0] != New_Center)
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
            int[] Center_Pixel = new int[3] { Total_R, Total_G, Total_B };


            Site Center_Site = One_Cluster[1];
            int Center_Distance = Count_Pixel_Distance( Center_Site, Center_Pixel);
            for (int index = 2; index < Cluster_Num; index++)
            {
                int New_Distance = Count_Pixel_Distance( One_Cluster[index], Center_Pixel);
                if (New_Distance < Center_Distance)
                {
                    Center_Distance = New_Distance;
                    Center_Site = One_Cluster[index];
                }
            }

            return Center_Site;
        }



        private static int Count_Pixel_Distance(Site Picture_Pixel_Site,int[] Center)
        {
            return (Math.Abs(Picture_Pixel[Picture_Pixel_Site.X, Picture_Pixel_Site.Y, 0] - Center[0]) +
                Math.Abs(Picture_Pixel[Picture_Pixel_Site.X, Picture_Pixel_Site.Y, 1] - Center[1]) +
                Math.Abs(Picture_Pixel[Picture_Pixel_Site.X, Picture_Pixel_Site.Y, 2] - Center[2]));
        }
    }
}
