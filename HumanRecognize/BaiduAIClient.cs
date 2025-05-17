using Baidu.Aip.Face;
using BaiduAIClientResponse;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanRecognize
{
    /// <summary>
    /// 百度AI客户端
    /// </summary>
    internal class BaiduAIClient
    {
        private readonly Face client = new(AppConfig.AccessKey, AppConfig.SecretKey);

        private static readonly BaiduAIClient Instance_ = new();
        public static BaiduAIClient Instance { get { return Instance_; } }

        /// <summary>
        /// 人脸检测
        /// </summary>
        /// <param name="imagePath">人脸图片路径</param>
        public FaceAnalysisResponse HumanFaceDetect(string imagePath)
        {
            var imageType = "BASE64";
            // 如果有可选参数
            var options = new Dictionary<string, object>{
                // 逗号分隔. 默认只返回face_token、人脸框、概率和旋转角度
                // age, 年龄
                // expression, 表情 type返回：none:不笑；smile:微笑；laugh:大笑；probability表情置信度，范围【0~1】，0最小、1最大。
                // face_shape, square: 正方形 triangle:三角形 oval: 椭圆 heart: 心形 round: 圆形
                // gender, 性别，face_field包含gender时返回 male:男性 female:女性
                // glasses, 是否带眼镜，face_field包含glasses时返回 none:无眼镜，common:普通眼镜，sun:墨镜
                // landmark,4个关键点位置，左眼中心、右眼中心、鼻尖、嘴中心。face_field包含landmark时返回
                // landmark72, 72个特征点位置 face_field包含landmark时返回
                // landmark150, 150个特征点位置 face_field包含landmark150时返回
                // quality, 人脸质量信息。face_field包含quality时返回
                // eye_status, 双眼状态（睁开/闭合） face_field包含eye_status时返回
                // emotion, 情绪 face_field包含emotion时返回 angry:愤怒 disgust:厌恶 fear:恐惧 happy:高兴 sad:伤心 surprise:惊讶 neutral:无情绪
                // face_type 真实人脸/卡通人脸 face_field包含face_type时返回  human: 真实人脸 cartoon: 卡通人脸
                { "face_field", "age,expression,face_shape,gender,quality,landmark,eye_status,emotion,face_type"},
                // 最多处理人脸的数目，默认值为1，仅检测图片中面积最大的那个人脸；最大值10，检测图片中面积最大的几张人脸。
                {"max_face_num", 1},
                // 人脸的类型 默认LIVE
                // LIVE表示生活照：通常为手机、相机拍摄的人像图片、或从网络获取的人像图片等
                // IDCARD表示身份证芯片照：二代身份证内置芯片中的人像照片
                // WATERMARK表示带水印证件照：一般为带水印的小图，如公安网小图
                // CERT表示证件照片：如拍摄的身份证、工卡、护照、学生证等证件图片
                {"face_type", "LIVE"},
                // 活体检测控制 默认NONE
                // NONE: 不进行控制
                // LOW:较低的活体要求(高通过率 低攻击拒绝率)
                // NORMAL: 一般的活体要求(平衡的攻击拒绝率, 通过率)
                // HIGH: 较高的活体要求(高攻击拒绝率 低通过率)
                {"liveness_control", "NONE"}
            };
            var image = AppUtil.ReadImageToBase64(imagePath);
            // 带参数调用人脸检测
            var result = client.Detect(image, imageType, options);
            return result.ToObject<FaceAnalysisResponse>();
        }

        /// <summary>
        /// 人脸比对
        /// </summary>
        /// <param name="leftImagePath">图片1路径</param>
        /// <param name="rightImagePath">图片2路径</param>
        public FaceComparisonResponse HumanFaceCompare(string leftImagePath, string rightImagePath)
        {
            var faces = new JArray
            {
                new JObject
                {
                    {"image", AppUtil.ReadImageToBase64(leftImagePath)},
                    {"image_type", "BASE64"},
                    {"face_type", "LIVE"},
                    // 图片质量控制 默认NONE
                    // NONE: 不进行控制
                    // LOW:较低的质量要求
                    // NORMAL: 一般的质量要求
                    // HIGH: 较高的质量要求
                    {"quality_control", "LOW"},
                    // 活体检测控制 默认NONE
                    // NONE: 不进行控制
                    // LOW:较低的活体要求(高通过率 低攻击拒绝率)
                    // NORMAL: 一般的活体要求(平衡的攻击拒绝率, 通过率)
                    // HIGH: 较高的活体要求(高攻击拒绝率 低通过率)
                    {"liveness_control", "NONE"},
                },
                new JObject
                {
                    {"image",AppUtil.ReadImageToBase64(rightImagePath)},
                    {"image_type", "BASE64"},
                    {"face_type", "LIVE"},
                    {"quality_control", "LOW"},
                    {"liveness_control", "NONE"},
                }
            };

            var result = client.Match(faces);
            return result.ToObject<FaceComparisonResponse>();
        }

    }

}

namespace BaiduAIClientResponse
{
    /// <summary>
    /// 人脸识别响应
    /// </summary>
    public class FaceAnalysisResponse
    {
        [JsonProperty("error_code")]
        public int errorCode { get; set; }

        [JsonProperty("error_msg")]
        public string errorMsg { get; set; }

        [JsonProperty("log_id")]
        public long logId { get; set; }

        [JsonProperty("timestamp")]
        public long timestamp { get; set; }

        [JsonProperty("cached")]
        public int cached { get; set; }

        [JsonProperty("result")]
        public Result result { get; set; }
    }

    public class Result
    {
        [JsonProperty("face_num")]
        public int faceNum { get; set; }

        [JsonProperty("face_list")]
        public List<FaceInfo> faceList { get; set; }
    }

    public class FaceInfo
    {
        [JsonProperty("face_token")]
        public string faceToken { get; set; }

        [JsonProperty("location")]
        public FaceLocation location { get; set; }

        [JsonProperty("face_probability")]
        public float faceProbability { get; set; }

        [JsonProperty("angle")]
        public FaceAngle angle { get; set; }

        [JsonProperty("landmark")]
        public List<LandmarkPoint> landmark { get; set; }

        [JsonProperty("landmark72")]
        public List<LandmarkPoint> landmark72 { get; set; }

        [JsonProperty("age")]
        public int age { get; set; }

        [JsonProperty("expression")]
        public FaceExpression expression { get; set; }

        [JsonProperty("gender")]
        public FaceGender gender { get; set; }

        [JsonProperty("eye_status")]
        public EyeStatus eyeStatus { get; set; }

        [JsonProperty("face_shape")]
        public FaceShape faceShape { get; set; }

        [JsonProperty("emotion")]
        public FaceEmotion emotion { get; set; }

        [JsonProperty("quality")]
        public FaceQuality quality { get; set; }

        [JsonProperty("face_type")]
        public FaceType faceType { get; set; }
    }

    public class FaceLocation
    {
        [JsonProperty("left")]
        public float left { get; set; }

        [JsonProperty("top")]
        public float top { get; set; }

        [JsonProperty("width")]
        public int width { get; set; }

        [JsonProperty("height")]
        public int height { get; set; }

        [JsonProperty("rotation")]
        public int rotation { get; set; }
    }

    public class FaceAngle
    {
        [JsonProperty("yaw")]
        public float yaw { get; set; }

        [JsonProperty("pitch")]
        public float pitch { get; set; }

        [JsonProperty("roll")]
        public float roll { get; set; }
    }

    public class LandmarkPoint
    {
        [JsonProperty("x")]
        public float x { get; set; }

        [JsonProperty("y")]
        public float y { get; set; }
    }

    public class FaceExpression
    {
        [JsonProperty("type")]
        public string type { get; set; }

        [JsonProperty("probability")]
        public float probability { get; set; }
    }

    public class FaceGender
    {
        [JsonProperty("type")]
        public string type { get; set; }

        [JsonProperty("probability")]
        public float probability { get; set; }
    }

    public class EyeStatus
    {
        [JsonProperty("left_eye")]
        public float leftEye { get; set; }

        [JsonProperty("right_eye")]
        public float rightEye { get; set; }
    }

    public class FaceShape
    {
        [JsonProperty("type")]
        public string type { get; set; }

        [JsonProperty("probability")]
        public float probability { get; set; }
    }

    public class FaceEmotion
    {
        [JsonProperty("type")]
        public string type { get; set; }

        [JsonProperty("probability")]
        public float probability { get; set; }
    }

    public class FaceQuality
    {
        [JsonProperty("occlusion")]
        public FaceOcclusion occlusion { get; set; }

        [JsonProperty("blur")]
        public int blur { get; set; }

        [JsonProperty("illumination")]
        public int illumination { get; set; }

        [JsonProperty("completeness")]
        public int completeness { get; set; }
    }

    public class FaceOcclusion
    {
        [JsonProperty("left_eye")]
        public float leftEye { get; set; }

        [JsonProperty("right_eye")]
        public float rightEye { get; set; }

        [JsonProperty("nose")]
        public float nose { get; set; }

        [JsonProperty("mouth")]
        public float mouth { get; set; }

        [JsonProperty("left_cheek")]
        public float leftCheek { get; set; }

        [JsonProperty("right_cheek")]
        public float rightCheek { get; set; }

        [JsonProperty("chin_contour")]
        public float chinContour { get; set; }
    }

    public class FaceType
    {
        [JsonProperty("type")]
        public string type { get; set; }

        [JsonProperty("probability")]
        public float probability { get; set; }
    }

    public class FaceComparisonResponse
    {
        [JsonProperty("error_code")]
        public int? errorCode { get; set; }

        [JsonProperty("error_msg")]
        public string errorMsg { get; set; }

        [JsonProperty("log_id")]
        public long? logId { get; set; }

        [JsonProperty("timestamp")]
        public long? timestamp { get; set; }

        [JsonProperty("cached")]
        public int? cached { get; set; }

        [JsonProperty("result")]
        public ComparisonResult result { get; set; }
    }

    public class ComparisonResult
    {
        [JsonProperty("score")]
        public double? score { get; set; }

        [JsonProperty("face_list")]
        public List<FaceCompareInfo> faceList { get; set; }
    }

    public class FaceCompareInfo
    {
        [JsonProperty("face_token")]
        public string faceToken { get; set; }
    }
}