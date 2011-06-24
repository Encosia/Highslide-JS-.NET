using System;
using System.ComponentModel;
using System.Web.UI;
using System.Text;

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
    
    public enum AnchorPositionType
    {
      Auto,
      Top,
      TopRight,
      Right,
      BottomRight,
      Bottom,
      BottomLeft,
      Left,
      TopLeft
    }

    public enum ExpandEventType
    {
      Click,
      MouseOver
    }

    // Global reference for HighslideImages to use.
    public static HighslideManager Manager = null;

    private bool _controlBar = true;
    private ControlBarPostitionType _controlBarPosition = ControlBarPostitionType.TopRight;

    private string _controlBarPreviousTitle = "Previous (left arrow key)";
    private string _controlBarNextTitle = "Next (right arrow key)";
    private string _controlBarMoveTitle = "Click and drag to move";
    private string _controlBarCloseTitle = "Close";

    private int _marginTop = 15;
    private int _marginRight = 15;
    private int _marginBottom = 15;
    private int _marginLeft = 15;

    private bool _showFullExpandButton = true;
    
    private bool? _allowMultipleInstances;
    private bool? _blockRightClick;

    private bool _fadeInOut = true;
    private OutlineTypes _outlineType = OutlineTypes.RoundedWhite;
    private bool _includeDefaultCSS = true;
    private int _numberOfImagesToPreload = 5;
    private AnchorPositionType _anchorPosition;
    private bool _centerEnlargements = false;
    private ExpandEventType _expandEvent = ExpandEventType.Click;

    [Description("Display the control bar on enlargements.")]
    [DefaultValue(true)]
    public bool ControlBar
    {
      get { return _controlBar; }
      set { _controlBar = value; }
    }

    [Description("Where the control bar should be positioned, relative to the enlarged image.")]
    [DefaultValue(ControlBarPostitionType.TopRight)]
    public ControlBarPostitionType ControlBarPosition
    {
      get { return _controlBarPosition; }
      set { _controlBarPosition = value; }
    }

    [Description("The title text that should appear for the previous button.")]
    [DefaultValue("Previous (left arrow key)")]
    public string ControlBarPreviousTitle
    {
      get { return _controlBarPreviousTitle; }
      set { _controlBarPreviousTitle = value; }
    }

    [Description("The title text that should appear for the next button.")]
    [DefaultValue("Next (right arrow key)")]
    public string ControlBarNextTitle
    {
      get { return _controlBarNextTitle; }
      set { _controlBarNextTitle = value; }
    }

    [Description("The title text that should appear for the move button.")]
    [DefaultValue("Click and drag to move")]
    public string ControlBarMoveTitle
    {
      get { return _controlBarMoveTitle; }
      set { _controlBarMoveTitle = value; }
    }

    [Description("The title text that should appear for the close button.")]
    [DefaultValue("Close")]
    public string ControlBarCloseTitle
    {
      get { return _controlBarCloseTitle; }
      set { _controlBarCloseTitle = value; }
    }

    [Description("A guaranteed offset between the enlargement and the top edge of the viewport.")]
    [DefaultValue(15)]
    public int MarginTop
    {
      get { return _marginTop; }
      set { _marginTop = value; }
    }

    [Description("A guaranteed offset between the enlargement and the right edge of the viewport.")]
    [DefaultValue(15)]
    public int MarginRight
    {
      get { return _marginRight; }
      set { _marginRight = value; }
    }

    [Description("A guaranteed offset between the enlargement and the bottom edge of the viewport.")]
    [DefaultValue(15)]
    public int MarginBottom
    {
      get { return _marginBottom; }
      set { _marginBottom = value; }
    }

    [Description("A guaranteed offset between the enlargement and the left edge of the viewport.")]
    [DefaultValue(15)]
    public int MarginLeft
    {
      get { return _marginLeft; }
      set { _marginLeft = value; }
    }

    [Description("Should the full expansion button be shown when the enlargement is scaled to fit available area?")]
    [DefaultValue(true)]
    public bool ShowFullExpandButton
    {
      get { return _showFullExpandButton; }
      set { _showFullExpandButton = value; }
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

    [Description("Which point of the thumbnail the enlargement expands from.")]
    [DefaultValue(AnchorPositionType.Auto)]
    public AnchorPositionType AnchorPosition
    {
      get { return _anchorPosition; }
      set { _anchorPosition = value; }
    }

    [Description("Should the enlargement be centered above the thumbnail?")]
    [DefaultValue(false)]
    public bool CenterEnlargements
    {
      get { return _centerEnlargements; }
      set { _centerEnlargements = value; }
    }

    [Description("Which event should trigger the thumbnail to expand?")]
    [DefaultValue(ExpandEventType.Click)]
    public ExpandEventType ExpandEvent
    {
      get { return _expandEvent; }
      set { _expandEvent = value; }
    }

    [Description("Should more than one image be allowed to enlarge at the same time?")]
    [DefaultValue(true)]
    public bool? AllowMultipleInstances
    {
      get { return _allowMultipleInstances; }
      set { _allowMultipleInstances = value; }
    }

    [Description("Should Highslide attempt to prevent right clicks on the image?")]
    [DefaultValue(false)]
    public bool? BlockRightClick
    {
      get { return _blockRightClick; }
      set { _blockRightClick = value; }
    }

    /// <summary>
    /// To improve serialization.  Later...
    /// </summary>
    private class HSExpanderOptionsDTO
    {
      public string outlineType;
      public string fadeInOut;
      public int numberOfImagesToPreload;
      public string anchor;

      public int marginTop;
      public int marginRight;
      public int marginBottom;
      public int marginLeft;
    }

    public HighslideManager()
    {
      // Set the static reference so that HighslideImages can
      //  easily check manager properties later in the page.
      Manager = this;
    }

    protected override void OnPreRender(EventArgs e)
    {
      if (!RenderScriptInPlace)
      {
        // Register the main JavaScript code, using embedded resource link.
        string hsEmbedSrc = Page.ClientScript.GetWebResourceUrl(GetType(), "HighslideImage.highslide.min.js");

        Page.ClientScript.RegisterClientScriptInclude("Highslide", hsEmbedSrc);
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

      StringBuilder options = new StringBuilder();

      options.AppendFormat("hs.outlineType = '{0}';", OutlineType);

      options.AppendFormat("hs.fadeInOut = {0};", FadeInOut ? "true" : "false");

      options.AppendFormat("hs.numberOfImagesToPreload = {0};", NumberOfImagesToPreload);

      options.AppendFormat("hs.align = '{0}';", CenterEnlargements ? "center" : "auto");

      options.AppendFormat("hs.anchor = '{0}';", GetAnchorPositionString(AnchorPosition));

      if (MarginTop != 15)
        options.AppendFormat("hs.marginTop = {0};", MarginTop);

      if (MarginRight != 15)
        options.AppendFormat("hs.marginRight = {0};", MarginRight);

      if (MarginBottom != 15)
        options.AppendFormat("hs.marginBottom = {0};", MarginBottom);

      if (MarginLeft != 15)
        options.AppendFormat("hs.marginLeft = {0};", MarginLeft);

      if (!ShowFullExpandButton)
        options.Append("hs.fullExpandOpacity = 0;");

      if (AllowMultipleInstances != null)
        options.AppendFormat("hs.allowMultipleInstances = {0};", AllowMultipleInstances.Value ? "true" : "false");

      if (BlockRightClick != null)
        options.AppendFormat("hs.blockRightClick = {0};", BlockRightClick.Value ? "true" : "false");

      if (ExpandEvent == ExpandEventType.MouseOver)
        options.Append("hs.Expander.prototype.onMouseOut = function(sender) { sender.close(); };");

      Page.ClientScript.RegisterStartupScript(GetType(), "HighslideOptions", options.ToString(), true);

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
        string hsEmbedSrc = Page.ClientScript.GetWebResourceUrl(GetType(), "HighslideImage.highslide.min.js");

        writer.WriteLine(string.Format("<script type=\"text/javascript\" src=\"{0}\"></script>", hsEmbedSrc));
      }

      if (ControlBar)
      {
        const string controlBarTemplate = "<div id=\"controlbar\" class=\"highslide-overlay controlbar\">" +
                                          "<a href=\"#\" class=\"previous\" onclick=\"return hs.previous(this)\" title=\"{0}\"></a>" +
                                          "<a href=\"#\" class=\"next\" onclick=\"return hs.next(this);\" title=\"{1}\"></a>" +
                                          "<a href=\"#\" class=\"highslide-move\" onclick=\"return false;\" title=\"{2}\"></a>" +
                                          "<a href=\"#\" class=\"close\" onclick=\"return hs.close(this);\" title=\"{3}\"></a>" +
                                          "</div>";

        string controlBarMarkup = string.Format(controlBarTemplate, ControlBarPreviousTitle, ControlBarNextTitle,
                                                ControlBarMoveTitle, ControlBarCloseTitle);

        writer.WriteLine(controlBarMarkup);
      }

      base.Render(writer);
    }

    /// <summary>
    /// Map a ControlBarPositionType enum to the string required by Highslide.
    /// </summary>
    private string GetControlBarPositionString(ControlBarPostitionType position)
    {
      string[] positionNames = { "top left", "top right", "bottom left", "bottom right" };

      return positionNames[(int)position];
    }

    /// <summary>
    /// Map a GetAnchorPosition enum to the string required by Highslide.
    /// </summary>
    private string GetAnchorPositionString(AnchorPositionType position)
    {
      string[] positionNames = { "auto", "top", "top right", "right", "bottom right",
                                 "bottom", "bottom left", "left", "top left" };

      return positionNames[(int)position];
    }
  }
}
