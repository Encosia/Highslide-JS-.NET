using System.ComponentModel;
using System.Web.UI;

namespace Encosia
{
  public class HighslideImage : System.Web.UI.WebControls.Image
  {
    private string _fullImageURL = "";
    private string _caption;
    private string _titleText = "Click to view full size";
    private string _linkTarget;
    
    [Description("URL to full size image when the thumbnail is clicked")]
    [UrlProperty()]
    public string FullImageURL
    {
      get { return _fullImageURL; }
      set { _fullImageURL = value; }
    }

    [Description("Caption text.")]
    [DefaultValue("")]
    public string Caption
    {
      get { return _caption; }
      set { _caption = value; }
    }
    
    [Description("Title text for the thumbnail image")]
    public string TitleText
    {
      get { return _titleText; }
      set { _titleText = value; }
    }
    
    [Description("Link target for users with JavaScript disabled.")]
    public string LinkTarget
    {
      get { return _linkTarget; }
      set { _linkTarget = value; }
    }

    protected override void Render(HtmlTextWriter writer)
    {
      // Adding attributes to the base (Image)
      writer.AddAttribute(HtmlTextWriterAttribute.Href, Page.ResolveUrl(_fullImageURL));
      writer.AddAttribute(HtmlTextWriterAttribute.Class, "highslide");
      
      if (!string.IsNullOrEmpty(_linkTarget))
        writer.AddAttribute(HtmlTextWriterAttribute.Target, _linkTarget);

      // Used for "unobtrusive" mode.
      //writer.AddAttribute(HtmlTextWriterAttribute.Rel, "highslide");

      // "Obtrusive" mode, I suppose.  Really, this is better for now, in case
      //  the HighslideImage is in an UpdatePanel.
      writer.AddAttribute(HtmlTextWriterAttribute.Onclick, "return hs.expand(this);");

      // Create the <a> tag and set up its attributes.
      writer.RenderBeginTag(HtmlTextWriterTag.A);

      if (!string.IsNullOrEmpty(_titleText))
        writer.AddAttribute(HtmlTextWriterAttribute.Title, _titleText);
      
      if (string.IsNullOrEmpty(AlternateText))
        writer.AddAttribute(HtmlTextWriterAttribute.Alt, "Thumbnail image for " + _fullImageURL);
      
      writer.AddAttribute(HtmlTextWriterAttribute.Class, "highslide");
      
      // Render the image, with the added attributes and <a> tag.
      base.Render(writer);

      // Render the </a> tag.
      writer.RenderEndTag();

      // If there's a caption set, render its <div>.
      if (_caption != "")
        writer.WriteLine(string.Format("<div class='highslide-caption'>{0}</div>", _caption));
    }
  }
}
