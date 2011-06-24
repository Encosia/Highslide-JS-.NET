<%@ Page Language="C#" AutoEventWireup="true"  CodeFile="Default.aspx.cs" Inherits="_Default" %>
<%@ Register Namespace="Encosia" Assembly="HighslideImage" TagPrefix="encosia" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
  <title>Highslide JS .NET Test</title>
  <link href="style.css" rel="stylesheet" type="text/css" />
</head>
<body>
  <form id="form1" runat="server">
  <encosia:HighslideManager runat="server" OutlineType="RoundedWhite" ControlBar="true" />
    
  <div id="page">
    <div id="header">
      <div id="headerimg">
        <a href="http://encosia.com">
          <img src="img/encosia-logo-trans.png" height="100" width="312" alt="Encosia - ASP.NET, AJAX, and more"
            title="Encosia logo and tagline" /></a>
      </div>
      <div id="RSSBlock">
        <a href="http://www.feedburner.com/fb/a/emailverifySubmit?feedId=654672&amp;loc=en_US"
          title="Click here to subscribe to Encosia via email." rel="nofollow" target="_blank">
          <img src="img/email-sub-icon.png" height="26" width="26" border="0" alt="Click here to subscribe to Encosia via email" /></a>
        <a href="http://feeds.encosia.com/Encosia" title="Click here to subscribe to the Encosia RSS feed."
          rel="nofollow" target="_blank">
          <img src="img/rss-sub-icon.png" height="26" width="26" border="0" alt="Click here to subscribe to the Encosia (full) RSS feed" /></a>
      </div>
    </div>
      
    <div id="content">
      <encosia:HighslideImage runat="server" 
        ImageUrl="~/img/highslide-usage-thumb.jpg"
        FullImageURL="~/img/highslide-usage-full.jpg" 
        Caption="A screenshot of a usage example." />

      <encosia:HighslideImage runat="server" 
        ImageUrl="~/img/encosia-thumb.jpg" 
        FullImageURL="~/img/encosia-full.jpg" 
        Caption="A screenshot of my website, as it looked last year." />
    </div>
    
  </div>
    
  </form>
</body>
</html>
