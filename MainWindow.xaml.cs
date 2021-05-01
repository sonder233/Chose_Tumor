using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfDemo.ListBoxShowImages.ViewModels;
using OpenCvSharp;


namespace ChoseTumor
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {
        ObservableCollection<ListBoxShowImagesViewModel> vm { get; set; }
        static String rootPath = @"G:\Brain_tumor\301配准完毕\dataset\301registration_jpg";
        List<String> peopleList;
        List<String> pathList;
        int nowIndex = 0;
        public MainWindow()
        {
            InitializeComponent();
            //initData();
            pathList =  ForeachFile();
            peopleList = getPeopleList();
            comboBox.ItemsSource = pathList;
            comboBox.SelectionChanged += cbxList_SelectedIndexChanged;//添加改变触发事件
        }

        int currentPeopleIndex = 0;
        private void cbxList_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            String chosePath = comboBox.SelectedValue.ToString();
            int index = comboBox.SelectedIndex;
            nowIndex = index;
            currentPeopleIndex = index;
            ChangeContent(chosePath);
            
        }
        ArrayList imgPath = new ArrayList();
        ArrayList imgName = new ArrayList();
        private void imageListBox_Loaded(object sender, RoutedEventArgs e)
        {
            vm = new ObservableCollection<ListBoxShowImagesViewModel>();

            
            List<String> temp_pathList = ForeachFile();
            DirectoryInfo dir = new DirectoryInfo(temp_pathList[0]);
            curPatient.Content = temp_pathList[0];
            foreach (FileInfo dChild in dir.GetFiles("*"))
            {
                imgPath.Add(dChild.FullName); //路径名+文件名
                imgName.Add(dChild.Name);     //文件名
            }
            
            //用正常人的逻辑排序
            IComparer fileNameComparer = new FilesNameComparerClass();
            imgPath.Sort(fileNameComparer);
            imgName.Sort(fileNameComparer);
            
            for (int i = 0; i < imgPath.Count; i++)
            {
                vm.Add(new ListBoxShowImagesViewModel() { Path = imgPath[i].ToString(), Name = imgName[i].ToString()});
            }
            imageListBox.DataContext = vm;//添加数据
        }

        

        public List<String> getPeopleList()
        {
            List<String> temp = new List<string>();
            DirectoryInfo theFolder = new DirectoryInfo(rootPath);
            DirectoryInfo[] dirInfo = theFolder.GetDirectories();//获取所在目录的文件夹

            foreach (DirectoryInfo NextFolder in dirInfo)
            {
                //获取患者姓名
                temp.Add(NextFolder.FullName.Split('\\')[5]);
            }
            return temp;
        }
        public static List<String> ForeachFile()
        {
            List<String> pathList = new List<string>();
            try
            {
                DirectoryInfo theFolder = new DirectoryInfo(rootPath);
                DirectoryInfo[] dirInfo = theFolder.GetDirectories();//获取所在目录的文件夹
                FileInfo[] file = theFolder.GetFiles();//获取所在目录的文件

                //foreach (FileInfo fileItem in file) //遍历文件
                //{
                //    //将文件信息存到列表
                //    pathList.Add(fileItem.FullName);
                    
                //}
                //遍历文件夹
                foreach (DirectoryInfo NextFolder in dirInfo)
                {
                    String temp_path = NextFolder.FullName;
                    DirectoryInfo theFolder_son = new DirectoryInfo(temp_path);
                    DirectoryInfo[] dirInfo_son = theFolder_son.GetDirectories();//获取所在目录的文件夹
                    foreach (DirectoryInfo NextFolder_son in dirInfo_son)
                    {
                        if (NextFolder_son.Name.Contains("flair"))
                        {
                            pathList.Add(NextFolder_son.FullName);
                        }
                    }
                        
                    //string[] sArray = NextFolder.FullName.Split('\\');
                    
                    
                }
                
            }
            catch (Exception)
            {
                throw;
            }
            return pathList;

        }

        String currentImgName;
        string currentMaskSavePath = "";
        string currentMaskSaveName = "";
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int number = imageListBox.SelectedIndex;
            string singelPath = imgPath[number].ToString();
            string singelImg = imgName[number].ToString();
            string temp = singelPath.Replace("301registration_jpg", "301registration_max");
            currentMaskSavePath = temp.Replace("\\flair\\" + singelImg, "");
            //图片所在路径
            //string finalPath = singelPath.Replace("\\"+imgName[number].ToString(), "");
            //getTenPic(finalPath, number);
            //MessageBox.Show(singelImg);
            PicCut(singelPath);
        }

        public void getTenPic(String path , int index)
        {
            List<String> list = new List<string>();
            for (int i = index-5; i < index+5; i++)
            {
                //10张需要拷贝的路径+文件名
                string sourcePath = path + "\\"+i+".jpg";
                string targetPath = sourcePath.Replace("301registration_jpg", "301registration_max");
                File.Copy(sourcePath, targetPath);
                list.Add(sourcePath);
            }
        }
        private MouseCallback MyMouseCallback;
        String testPath = @"G:\Brain_tumor\301samp_后20_jpg\huozengyu\flair\39.jpg";
        Mat img;
        /// <summary>
        /// 裁切选中图片
        /// </summary>
        /// <param name="path">图片路径</param>
        public void PicCut(String path)
        {
            img = new Mat(path, ImreadModes.Color);
            Cv2.ImShow("chosenImg", img);
            //Cv2.SetMouseCallback("img", img_MouseDown);

            MyMouseCallback = new MouseCallback(img_MouseDown);
            Cv2.MoveWindow("chosenImg", 800, 400);
            Cv2.SetMouseCallback("chosenImg", MyMouseCallback);
            Cv2.WaitKey(0);
            Cv2.DestroyWindow("chosenImg");
            Cv2.DestroyAllWindows();
        }

       
        
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            nowIndex++;
            ChangeContent(pathList[nowIndex]);
            comboBox.SelectedIndex = nowIndex;
            if (pathList[nowIndex].Contains("flair"))
            {
                maskButton.IsEnabled = true;
            }
            else
            {
                maskButton.IsEnabled = false;
            }
        }

       /// <summary>
       /// 鼠标点击事件
       /// </summary>
        private void img_MouseDown(MouseEventTypes @event, int x, int y, MouseEventFlags flags, IntPtr userData)
        {
            if (@event.ToString().Equals("LButtonDown"))
            {   
                int y0 = y - 40;
                int x0 = x - 40;
                int x1 = x0 + 80;
                int y1 = y0 + 80;
                //MessageBox.Show(x.ToString());
                OpenCvSharp.Point p0 = new OpenCvSharp.Point(x0, y0);
                OpenCvSharp.Point p1 = new OpenCvSharp.Point(x1, y1);
                getMask(p0, p1,peopleList[nowIndex/4]);
                //Cv2.Rectangle(img, p0, p1,Scalar.Red);
                //Cv2.ImShow("chosenImg", img);
                Cv2.WaitKey(0);
                //Cv2.DestroyWindow("chosenImg");
            }
        }

        /// <summary>
        /// 获得遮罩并保存
        /// </summary>
        /// <param name="p0"></param>
        /// <param name="p1"></param>
        public void getMask(OpenCvSharp.Point p0, OpenCvSharp.Point p1,String peopleName)
        {
            OpenCvSharp.Size size = new OpenCvSharp.Size(240,240);
            Mat whiteMask = new Mat(size,MatType.CV_8UC3,Scalar.Black);
            Cv2.Rectangle(whiteMask, p0, p1, Scalar.White,-1);
            Cv2.ImShow("mask", whiteMask);
            Cv2.ImWrite(currentMaskSavePath +"\\"+ peopleName + "_mask.jpg", whiteMask);
            Cv2.WaitKey(1);
            
        }
        public void ChangeContent(String path)
        {
            vm = new ObservableCollection<ListBoxShowImagesViewModel>();
            curPatient.Content = path;
            
            DirectoryInfo dir = new DirectoryInfo(path);
            imgPath.Clear();
            imgName.Clear();
            foreach (FileInfo dChild in dir.GetFiles("*"))
            {
                imgPath.Add(dChild.FullName); //路径名+文件名
                imgName.Add(dChild.Name);     //文件名
            }

            //用正常人的逻辑排序
            IComparer fileNameComparer = new FilesNameComparerClass();
            imgPath.Sort(fileNameComparer);
            imgName.Sort(fileNameComparer);

            for (int i = 0; i < imgPath.Count; i++)
            {
                vm.Add(new ListBoxShowImagesViewModel() { Path = imgPath[i].ToString(), Name = imgName[i].ToString() });
            }
            imageListBox.DataContext = vm;//添加数据
        }
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            OpenCvSharp.Point p0 = new OpenCvSharp.Point(10,80);
            OpenCvSharp.Point p1 = new OpenCvSharp.Point(100, 100);
            //getMask(p0, p1);
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            int number = imageListBox.SelectedIndex;
            string singelPath = imgPath[number].ToString();
            string singelImg = imgName[number].ToString();
            //图片所在路径
            string finalPath = singelPath.Replace("\\" + imgName[number].ToString(), "");
            getTenPic(finalPath, number);
            //MessageBox.Show(singelImg);
            //PicCut(singelPath);
        }
    }

    public class FilesNameComparerClass : IComparer
    {
        // Calls CaseInsensitiveComparer.Compare with the parameters reversed.
        ///<summary>
        ///比较两个字符串，如果含用数字，则数字按数字的大小来比较。
        ///</summary>
        ///<param name="x"></param>
        ///<param name="y"></param>
        ///<returns></returns>
        int IComparer.Compare(Object x, Object y)
        {
            if (x == null || y == null)
                throw new ArgumentException("Parameters can't be null");
            string fileA = x as string;
            string fileB = y as string;
            char[] arr1 = fileA.ToCharArray();
            char[] arr2 = fileB.ToCharArray();
            int i = 0, j = 0;
            while (i < arr1.Length && j < arr2.Length)
            {
                if (char.IsDigit(arr1[i]) && char.IsDigit(arr2[j]))
                {
                    string s1 = "", s2 = "";
                    while (i < arr1.Length && char.IsDigit(arr1[i]))
                    {
                        s1 += arr1[i];
                        i++;
                    }
                    while (j < arr2.Length && char.IsDigit(arr2[j]))
                    {
                        s2 += arr2[j];
                        j++;
                    }
                    if (int.Parse(s1) > int.Parse(s2))
                    {
                        return 1;
                    }
                    if (int.Parse(s1) < int.Parse(s2))
                    {
                        return -1;
                    }
                }
                else
                {
                    if (arr1[i] > arr2[j])
                    {
                        return 1;
                    }
                    if (arr1[i] < arr2[j])
                    {
                        return -1;
                    }
                    i++;
                    j++;
                }
            }
            if (arr1.Length == arr2.Length)
            {
                return 0;
            }
            else
            {
                return arr1.Length > arr2.Length ? 1 : -1;
            }
            //            return string.Compare( fileA, fileB );
            //            return( (new CaseInsensitiveComparer()).Compare( y, x ) );
        }
    }

}
