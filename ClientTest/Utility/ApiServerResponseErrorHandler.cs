using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ClientTest.Utility
{
    public static class ApiServerResponseErrorHandler
    {
        /// <summary>
        /// HTTP応答をチェックする。
        /// 応答が正常でなければ、例外を投げる
        /// </summary>
        /// <param name="res">HttpClientから応答を受け取る</param>
        /// <exception cref="ApplicationException">500:サーバー内の状態が不正。</exception>
        public static async Task CheckResponseStatus(HttpResponseMessage res)
        {
            const String templateMsg = "コード:{0} ({1})\n詳細:{2}";
            const String templateExtraMsg = "コード:{0} ({1})\n{2}\n詳細:{3}";

            //応答ステータスチェック
            if (!res.IsSuccessStatusCode)
            {
                //中身をロード
                var content = await res.Content.ReadAsStringAsync();
                String details = null;

                //エラーを解析
                //Contentがないなら
                if(!String.IsNullOrEmpty(content))
                    details = HandleErrorDetails(content);

                ApplicationException ex;

                switch (res.StatusCode)
                {
                    case HttpStatusCode.BadRequest:
                        ex = new ApplicationException(String.Format(templateMsg,400,"不正な入力", details));
                        ex.Data.Add("details", details);
                        ex.Data.Add("ResponseCode", 400);
                        throw ex;
                    case HttpStatusCode.Unauthorized:
                        ex = new ApplicationException(String.Format(templateExtraMsg,401,"認証に失敗","再ログインしてください。", details));
                        ex.Data.Add("ResponseCode",401);
                        throw ex;
                    case HttpStatusCode.NotFound:
                        ex = new ApplicationException(String.Format(templateExtraMsg, 404, "リソース獲得失敗", "要求されたリソースがありませんでした。", details));
                        ex.Data.Add("ResponseCode", 404);
                        throw ex;
                    case HttpStatusCode.InternalServerError:
                        ex = new ApplicationException(String.Format(templateExtraMsg,500,"サーバー内で不正な処理が発生","サポートへご連絡お願いします。", details));
                        ex.Data.Add("ResponseCode", 500);
                        throw ex;
                    case HttpStatusCode.ServiceUnavailable:
                        ex = new ApplicationException(String.Format(templateExtraMsg,503,"サーバーが一時的に利用できません","ネットワークの状況を確認してください。", details));
                        ex.Data.Add("ResponseCode", 503);
                        throw ex;

                    default:
                        throw new ApplicationException("想定してないエラーが発生しました。\nサポートへご連絡お願いします。");
                }
            }
        }

        public static String HandleErrorDetails(String content)
        {
            String errorDetails = "";

            //エラーの種類は4(+GetErrorResultとChallengeResult)種類
            //ModelStateDictionary型(MessageとModelState): BadRequestにModelStateなど
            //"error"と"error_description"の組：ログインの時のみ
            //"Message","ExceptionMessage",etcの組：例外を投げたとき（InternalServerErrorのときなど）
            //"Message"のみ：BadRequestに文字列のみ渡したとき

            //ModelState型の利用
            // Create an anonymous object to use as the template for deserialization:
            var anonymousModelStateErrorObject = new { Message = "", ModelState = new Dictionary<string, string[]>() };
            var msObj = JsonConvert.DeserializeAnonymousType(content, anonymousModelStateErrorObject);
            // Sometimes, there may be Model Errors:
            if (msObj.ModelState != null)
            {
                var errors =
                    msObj.ModelState.Select(kvp => string.Join(". ", kvp.Value));
                for (int i = 0; i < errors.Count(); i++)
                {
                    // Wrap the errors up into the base Exception.Data Dictionary:
                    //ex.Data.Add(i, errors.ElementAt(i));
                    errorDetails += errors.ElementAt(i).ToString() + "\n";
                }
                return errorDetails;
            }
             
            //err_dec型の利用
            var anonymousErrorDescriptionObject = new { error = "", error_description = "" };
            var errDecObj = JsonConvert.DeserializeAnonymousType(content, anonymousErrorDescriptionObject);
            if (errDecObj.error_description != null)
            {
                return errDecObj.error_description;
            }
            
            //Message,ExceptionMessage型の利用
            var anonymousExceptionMessageErrorObject = new { Message = "", ExceptionMessage = "" };
            var exMsgObj = JsonConvert.DeserializeAnonymousType(content, anonymousExceptionMessageErrorObject);
            if (exMsgObj.ExceptionMessage != null)
            {
                return exMsgObj.ExceptionMessage;
            }

            //
            //// Othertimes, there may not be Model Errors:
            //else
            //{
            //    var error =
            //        JsonConvert.DeserializeObject<Dictionary<string, string>>(content);
            //    foreach (var kvp in error)
            //    {
            //        // Wrap the errors up into the base Exception.Data Dictionary:
            //        ex.Data.Add(kvp.Key, kvp.Value);
            //    }
            //}

            //空欄だった。
            return "";
        }
    }
}
