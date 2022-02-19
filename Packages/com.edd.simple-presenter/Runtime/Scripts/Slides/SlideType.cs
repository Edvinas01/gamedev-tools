using UnityEngine;

namespace SimplePresenter.Slides
{
    public enum SlideType
    {
        [Tooltip("Empty slide with no content")]
        Empty,
        
        [Tooltip("Slide with a full page title")]
        Title,
        
        [Tooltip("Slide with a title and simple text")]
        TextContent,
        
        [Tooltip("Slide with a title and a custom prefab as content")]
        CustomContent
    }
}
