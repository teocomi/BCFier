using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace StatHat
{
  // Delegates
  public delegate void ReplyDelegate(string reply);

  public static class Post
  {

    // ===================
    // How to use StatHat:
    // ===================
    //
    // 1. Add StatHat.cs to your .NET project.
    // 2. Call its functions!
    //
    // -----------------------------------------------------------------------------------
    //
    // A simple example of posting a Counter:
    //
    //    StatHat.Post.Counter("FERF34fREF3443432","23FSDfEFWFEF22323", 9.95);
    //
    // -----------------------------------------------------------------------------------
    //
    // A simple example of posting a Value:
    //
    //    StatHat.Post.Value("FERF34fREF3443432","23FSDfEFWFEF22323", 512.2);
    //
    // -----------------------------------------------------------------------------------
    //
    // A simple example of posting a counter with our EZ API - this registers the stat automatically
    // if it doesn't exist (and registers you for the site if you don't have a membership):
    //
    //    StatHat.Post.EzCounter("you@example.com","dollars earned", 9.95);
    //
    // -----------------------------------------------------------------------------------
    //
    // If you care to read what the server is replying to your call (for error handling, curiosity, etc.)
    // you can pass a delegate function expecting a string for callback. Like so:
    //
    //
    //    // Here's some function I want StatHat to call with the reply:
    //    // ---------------------------------------------------
    //    void PrintStatHatReply(string reply) { Console.WriteLine(reply); }
    //
    //    // Make a delegate out of it.
    //    // -----------------------------
    //    StatHat.ReplyDelegate myDelegate = new StatHat.ReplyDelegate(PrintStatHatReply);
    //
    //    // Pass that delegate as a parameter:
    //    // ----------------------------------
    //    StatHat.Post.Counter("FERF34fREF3443432","23FSDfEFWFEF22323", 1.0, myDelegate);
    //
    //

    private const string BaseUrl = "http://api.stathat.com";

    /// <summary>
    /// Posts a counter increment to stathat over HTTP
    /// </summary>
    /// <param name="key">the stat's posting key</param>
    /// <param name="ukey">your user key</param>
    /// <param name="count">the number to increment</param>
    public static void Counter(string key, string ukey, float count)
    {
      Dictionary<string, string> p = new Dictionary<string, string>();
      p.Add("key", key);
      p.Add("ukey", ukey);
      p.Add("count", count.ToString());
      new FormPoster(Post.BaseUrl, "/c", p);
    }


    /// <summary>
    /// Posts a counter increment to stathat over HTTP
    /// </summary>
    /// <param name="key">the stat's posting key</param>
    /// <param name="ukey">your user key</param>
    /// <param name="count">the number to increment</param>
    public static void Counter(string key, string ukey, int count)
    {
      Post.Counter(key, ukey, (float)count);

    }
    /// <summary>
    /// Posts a value to stathat over HTTP
    /// </summary>
    /// <param name="key">the stat's posting key</param>
    /// <param name="ukey">your user key</param>
    /// <param name="value">the number</param>
    public static void Value(string key, string ukey, float value)
    {
      Dictionary<string, string> p = new Dictionary<string, string>();
      p.Add("key", key);
      p.Add("ukey", ukey);
      p.Add("value", value.ToString());
      new FormPoster(Post.BaseUrl, "/v", p);
    }
    /// <summary>
    /// Posts a value to stathat over HTTP
    /// </summary>
    /// <param name="key">the stat's posting key</param>
    /// <param name="ukey">your user key</param>
    /// <param name="value">the number</param>
    public static void Value(string key, string ukey, int value)
    {
      Post.Value(key, ukey, (float)value);
    }

    /// <summary>
    /// Posts a counter increment to stathat over HTTP using ez API - the stat and/or you don't have to be pre-registered
    /// </summary>
    /// <param name="ezkey">your ezkey (defaults to email address).  If you already have a stathat account, use the one associated with it.</param>
    /// <param name="stat">the name for your stat</param>
    /// <param name="count">the number to increment</param>
    public static void EzCounter(string ezkey, string stat, float count)
    {
      Dictionary<string, string> p = new Dictionary<string, string>();
      p.Add("ezkey", ezkey);
      p.Add("stat", stat);
      p.Add("count", count.ToString());
      new FormPoster(Post.BaseUrl, "/ez", p);
    }

    /// <summary>
    /// Posts a counter increment to stathat over HTTP using ez API - the stat and/or you don't have to be pre-registered
    /// </summary>
    /// <param name="ezkey">your ezkey (defaults to email address).  If you already have a stathat account, use the one associated with it.</param>
    /// <param name="stat">the name for your stat</param>
    /// <param name="count">the number to increment</param>
    public static void EzCounter(string ezkey, string stat, int count)
    {
      Post.EzCounter(ezkey, stat, (float)count);
    }

    /// <summary>
    /// Posts a counter increment to stathat over HTTP using ez API - the stat and/or you don't have to be pre-registered
    /// </summary>
    /// <param name="ezkey">your ezkey (defaults to email address).  If you already have a stathat account, use the one associated with it.</param>
    /// <param name="stat">the name for your stat</param>
    /// <param name="count">the number</param>
    public static void EzValue(string ezkey, string stat, float value)
    {
      Dictionary<string, string> p = new Dictionary<string, string>();
      p.Add("ezkey", ezkey);
      p.Add("stat", stat);
      p.Add("value", value.ToString());
      new FormPoster(Post.BaseUrl, "/ez", p);
    }

    /// <summary>
    /// Posts a counter increment to stathat over HTTP using ez API - the stat and/or you don't have to be pre-registered
    /// </summary>
    /// <param name="ezkey">your ezkey (defaults to email address).  If you already have a stathat account, use the one associated with it.</param>
    /// <param name="stat">the name for your stat</param>
    /// <param name="count">the number</param>
    public static void EzValue(string ezkey, string stat, int value)
    {
      Post.EzValue(ezkey, stat, (float)value);
    }

    /// <summary>
    /// Posts a counter increment to stathat over HTTP
    /// </summary>
    /// <param name="key">the stat's posting key</param>
    /// <param name="ukey">your user key</param>
    /// <param name="count">the number to increment</param>
    /// <param name="replyDelegate">the function you'd like called with the reply from stathat's server</param>
    public static void Counter(string key, string ukey, float count, ReplyDelegate replyDelegate)
    {
      Dictionary<string, string> p = new Dictionary<string, string>();
      p.Add("key", key);
      p.Add("ukey", ukey);
      p.Add("count", count.ToString());
      new FormPoster(Post.BaseUrl, "/c", p, replyDelegate);
    }
    /// <summary>
    /// Posts a counter increment to stathat over HTTP
    /// </summary>
    /// <param name="key">the stat's posting key</param>
    /// <param name="ukey">your user key</param>
    /// <param name="count">the number to increment</param>
    /// <param name="replyDelegate">the function you'd like called with the reply from stathat's server</param>
    public static void Counter(string key, string ukey, int count, ReplyDelegate replyDelegate)
    {
      Post.Counter(key, ukey, (float)count, replyDelegate);

    }
    /// <summary>
    /// Posts a value to stathat over HTTP
    /// </summary>
    /// <param name="key">the stat's posting key</param>
    /// <param name="ukey">your user key</param>
    /// <param name="value">the number</param>
    /// <param name="replyDelegate">the function you'd like called with the reply from stathat's server</param>
    public static void Value(string key, string ukey, float value, ReplyDelegate replyDelegate)
    {
      Dictionary<string, string> p = new Dictionary<string, string>();
      p.Add("key", key);
      p.Add("ukey", ukey);
      p.Add("value", value.ToString());
      new FormPoster(Post.BaseUrl, "/v", p, replyDelegate);
    }
    /// <summary>
    /// Posts a value to stathat over HTTP
    /// </summary>
    /// <param name="key">the stat's posting key</param>
    /// <param name="ukey">your user key</param>
    /// <param name="value">the number</param>
    /// <param name="replyDelegate">the function you'd like called with the reply from stathat's server</param>
    public static void Value(string key, string ukey, int value, ReplyDelegate replyDelegate)
    {
      Post.Value(key, ukey, (float)value, replyDelegate);
    }

    /// <summary>
    /// Posts a counter increment to stathat over HTTP using ez API - the stat and/or you don't have to be pre-registered
    /// </summary>
    /// <param name="ezkey">your ezkey (defaults to email address).  If you already have a stathat account, use the one associated with it.</param>
    /// <param name="stat">the name for your stat</param>
    /// <param name="count">the number to increment</param>
    /// <param name="replyDelegate">the function you'd like called with the reply from stathat's server</param>
    public static void EzCounter(string ezkey, string stat, float count, ReplyDelegate replyDelegate)
    {
      Dictionary<string, string> p = new Dictionary<string, string>();
      p.Add("ezkey", ezkey);
      p.Add("stat", stat);
      p.Add("count", count.ToString());
      new FormPoster(Post.BaseUrl, "/ez", p, replyDelegate);
    }

    /// <summary>
    /// Posts a counter increment to stathat over HTTP using ez API - the stat and/or you don't have to be pre-registered
    /// </summary>
    /// <param name="ezkey">your ezkey (defaults to email address).  If you already have a stathat account, use the one associated with it.</param>
    /// <param name="stat">the name for your stat</param>
    /// <param name="count">the number to increment</param>
    /// <param name="replyDelegate">the function you'd like called with the reply from stathat's server</param>
    public static void EzCounter(string ezkey, string stat, int count, ReplyDelegate replyDelegate)
    {
      Post.EzCounter(ezkey, stat, (float)count, replyDelegate);
    }

    /// <summary>
    /// Posts a counter increment to stathat over HTTP using ez API - the stat and/or you don't have to be pre-registered
    /// </summary>
    /// <param name="ezkey">your ezkey (defaults to email address).  If you already have a stathat account, use the one associated with it.</param>
    /// <param name="stat">the name for your stat</param>
    /// <param name="count">the number</param>
    /// <param name="replyDelegate">the function you'd like called with the reply from stathat's server</param>
    public static void EzValue(string ezkey, string stat, float value, ReplyDelegate replyDelegate)
    {
      Dictionary<string, string> p = new Dictionary<string, string>();
      p.Add("ezkey", ezkey);
      p.Add("stat", stat);
      p.Add("value", value.ToString());
      new FormPoster(Post.BaseUrl, "/ez", p, replyDelegate);
    }

    /// <summary>
    /// Posts a counter increment to stathat over HTTP using ez API - the stat and/or you don't have to be pre-registered
    /// </summary>
    /// <param name="ezkey">your ezkey (defaults to email address).  If you already have a stathat account, use the one associated with it.</param>
    /// <param name="stat">the name for your stat</param>
    /// <param name="count">the number</param>
    /// <param name="replyDelegate">the function you'd like called with the reply from stathat's server</param>
    public static void EzValue(string ezkey, string stat, int value, ReplyDelegate replyDelegate)
    {
      Post.EzValue(ezkey, stat, (float)value, replyDelegate);
    }

    private class FormPoster
    {
      // Members
      HttpWebRequest Request;
      Dictionary<string, string> Parameters;
      ReplyDelegate Reply;
      string RelUrl;
      string BaseUrl;


      // Methods
      public FormPoster(string base_url, string rel_url, Dictionary<string, string> parameters, ReplyDelegate replyDelegate)
      {
        this.BaseUrl = base_url;
        this.Parameters = parameters;
        this.Reply = replyDelegate;
        this.RelUrl = rel_url;
        this.PostForm();
      }
      public FormPoster(string base_url, string rel_url, Dictionary<string, string> parameters)
      {
        this.BaseUrl = base_url;
        this.Parameters = parameters;
        this.Reply = new ReplyDelegate((rep) => { });
        this.RelUrl = rel_url;
        this.PostForm();
      }

      private void PostForm()
      {
        this.Request = (HttpWebRequest)WebRequest.Create(this.BaseUrl + this.RelUrl);
        Request.Method = "POST";
        Request.ContentType = "application/x-www-form-urlencoded";
        Request.BeginGetRequestStream(this.RequestCallback, Request);
      }
      private void RequestCallback(IAsyncResult asyncResult)
      {
        try
        {
          string postData = "";
          foreach (string key in this.Parameters.Keys)
          {
            postData += encodeUriComponent(key) + "=" + encodeUriComponent(this.Parameters[key]) + "&";
          }
          Stream newStream = Request.EndGetRequestStream(asyncResult);
          StreamWriter streamWriter = new StreamWriter(newStream);
          streamWriter.Write(postData);
          streamWriter.Close();
          this.Request.BeginGetResponse(this.ResponseCallback, this.Request);
        }
        catch (Exception e)
        {
          this.Reply(e.Message);
        }
        finally { }
      }

      private string encodeUriComponent(string s)
      {
        string res = s.Replace("&", "%26");
        res = res.Replace(" ", "%20");
        return res;
      }

      private void ResponseCallback(IAsyncResult asyncResult)
      {
        try
        {
          WebResponse response = this.Request.EndGetResponse(asyncResult);
          Stream dataStream = response.GetResponseStream();
          StreamReader reader = new StreamReader(dataStream);
          string result = reader.ReadToEnd();
          this.Reply(result);
        }
        catch (Exception e)
        {
          this.Reply(e.Message);
        }
        finally { }
      }
    }
  }
}