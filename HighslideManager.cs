using System;
using System.ComponentModel;
using System.Web.UI;

namespace Encosia
{
  public class HighslideManager : Control
  {
    public enum OutlineTypes
    {
      Beveled,
      DropShadow,
      OuterGlow,
      RoundedWhite,
    }

    public enum ControlBarPostitionType
    {
      TopLeft,
      TopRight,
      BottomLeft,
      BottomRight,
    }

    private ControlBarPostitionType _controlBarPosition = ControlBarPostitionType.TopRight;
    private bool _fadeInOut = true;
    private OutlineTypes _outlineType = OutlineTypes.RoundedWhite;
    private bool _includeDefaultCSS = true;
    private int _numberOfImagesToPreload = 5;

    [Description("Display the control bar on enlargements.")]
    [DefaultValue(true)]
    public bool ControlBar { get; set; }

    [Description("Where the control bar should be positioned, relative to the enlarged image.")]
    [DefaultValue(ControlBarPostitionType.TopRight)]
    public ControlBarPostitionType ControlBarPosition
    {
      get { return _controlBarPosition; }
      set { _controlBarPosition = value; }
    }

    [Description("Fade the enlargement while it animates.")]
    [DefaultValue(true)]
    public bool FadeInOut
    {
      get { return _fadeInOut; }
      set { _fadeInOut = value; }
    }

    [Description("The outline style of the border around enlarged images.")]
    [DefaultValue(OutlineTypes.RoundedWhite)]
    public OutlineTypes OutlineType
    {
      get { return _outlineType; }
      set { _outlineType = value; }
    }

    [Description("If true, the <script> include will be rendered at the location of the HighslideManager.")]
    [DefaultValue(false)]
    public bool RenderScriptInPlace { get; set; }

    [Description("Should the HighslideManager include default CSS styling for Highslide elements?")]
    [DefaultValue(true)]
    public bool IncludeDefaultCSS
    {
      get { return _includeDefaultCSS; }
      set { _includeDefaultCSS = value; }
    }

    [Description("How many images to preload.  Use 0 to disable.  Defaults to 5.")]
    [DefaultValue(5)]
    public int NumberOfImagesToPreload
    {
      get { return _numberOfImagesToPreload; }
      set { _numberOfImagesToPreload = value; }
    }

    protected override void OnPreRender(EventArgs e)
    {
      if (!RenderScriptInPlace)
      {
        // Register the main JavaScript code, using embedded resource link.
        string HSEmbedSrc = Page.ClientScript.GetWebResourceUrl(GetType(), "HighslideImage.highslide.min.js");

        Page.ClientScript.RegisterClientScriptInclude("Highslide", HSEmbedSrc);
      }

      if (IncludeDefaultCSS)
      {
        if (Page.Header != null)
        {
          // Register the CSS styles, using embedded resource link.
          const string incTemplate = "<link rel=\"stylesheet\" type=\"text/css\" href=\"{0}\" />";
          string incLoc = Page.ClientScript.GetWebResourceUrl(GetType(), "HighslideImage.Highslide.css");

          // To force standards compliance.
          incLoc = incLoc.Replace("&t", "&amp;t");

          LiteralControl inc = new LiteralControl(string.Format(incTemplate, incLoc));

          Page.Header.Controls.Add(inc);
        }
        else
        {
          throw new Exception("Unable to access <head>, when attempting to register the Highslide CSS include.\n\n" +
                              "If using a master page, make sure its <head> includes a runat=\"server\" attribute.");
        }
      }

      // Set options in JavaScript block, based on properties.
      string options = string.Format("hs.outlineType = '{0}'; hs.fadeInOut = {1}; hs.numberOfImagesToPreload = {2};", 
        OutlineType, FadeInOut ? "true" : "false", NumberOfImagesToPreload);

      Page.ClientScript.RegisterStartupScript(GetType(), "HighslideOptions", options, true);

      if (ControlBar)
      {
        const string hsOptionsTemplate = "hs.registerOverlay({ thumbnailId: null, " +
                                                              "overlayId: 'controlbar', " +
                                                              "position: '$position' });";

        string script = hsOptionsTemplate.Replace("$position", GetControlBarPositionString(ControlBarPosition));

        Page.ClientScript.RegisterStartupScript(GetType(), "ControlBarOptions", script, true);
      }

      base.OnPreRender(e);
    }

    protected override void Render(HtmlTextWriter writer)
    {
      if (RenderScriptInPlace)
      {        
        // Register the main JavaScript code, using embedded resource link.
        string HSEmbedSrc = Page.ClientScript.GetWebResourceUrl(GetType(), "HighslideImage.highslide.min.js");

        writer.WriteLine(string.Format("<script type=\"text/javascript\" src=\"{0}\"></script>", HSEmbedSrc));
      }

      if (ControlBar)
      {
        writer.WriteLine("<div id=\"controlbar\" class=\"highslide-overlay controlbar\">" +
                         "<a href=\"#\" class=\"previous\" onclick=\"return hs.previous(this)\" title=\"Previous (left arrow key)\"></a>" +
                         "<a href=\"#\" class=\"next\" onclick=\"return hs.next(this);\" title=\"Next (right arrow key)\"></a>" +
                         "<a href=\"#\" class=\"highslide-move\" onclick=\"return false;\" title=\"Click and drag to move\"></a>" +
                         "<a href=\"#\" class=\"close\" onclick=\"return hs.close(this);\" title=\"Close\"></a>" +
                         "</div>");
      }

      base.Render(writer);
    }

    private string GetControlBarPositionString(ControlBarPostitionType position)
    {
      switch (position)
      {
        case ControlBarPostitionType.TopLeft:
          return "top left";

        case ControlBarPostitionType.BottomRight:
          return "bottom right";

        case ControlBarPostitionType.BottomLeft:
          return "bottom left";

        case ControlBarPostitionType.TopRight:
        default:
          return "top right";
      }
    }
  }
}
