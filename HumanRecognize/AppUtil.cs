using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanRecognize
{
    internal class AppUtil
    {
        /// <summary>
        /// 从文件系统中加载图片，并将图片内容包装为Base64字符串返回
        /// </summary>
        /// <param name="pictureLocation">图片位置</param>
        /// <returns>图片Base64内容</returns>
        public static string ReadImageToBase64(string pictureLocation)
        {
            var imageBytes = File.ReadAllBytes(pictureLocation);
            string base64String = Convert.ToBase64String(imageBytes);
            return base64String;
        }
    }
}
