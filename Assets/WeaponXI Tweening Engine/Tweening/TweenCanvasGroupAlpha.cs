using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TweenCanvasGroupAlpha : UITweener
{
    [Space(15)]
    [Range(0f, 1f)]
    public float from = 1f;
    [Range(0f, 1f)]
    public float to = 1f;

    bool mCached = false;
    SpriteRenderer mRect;
    Material mMat;
    CanvasGroup mSr;
    Text mText;
    Image mImage;

    [System.Obsolete("Use 'value' instead")]
    public float alpha { get { return this.value; } set { this.value = value; } }

    void Cache()
    {
        mCached = true;
        mSr = GetComponent<CanvasGroup>();

        if (mSr == null) return;

        //mText = GetComponent<Text>();

        //if (mText == null) return;

        //mImage = GetComponent<Image>();

        //if (mImage == null) return;
    }

    /// <summary>
    /// Tween's current value.
    /// </summary>

    public float value
    {
        get
        {
            if (!mCached) Cache();
            //if (mText != null) return mText.color.a;
            //if (mImage != null) return mImage.color.a;
            return mSr.alpha;
            //return mMat != null ? mMat.color.a : 1f;
        }
        set
        {
            if (!mCached) Cache();

            //if (mImage != null)
            //{
            //    Color c = mImage.color;
            //    c.a = value;
            //    mImage.color = c;
            //}
            if (mSr != null)
            {
                CanvasGroup c = mSr;
                c.alpha = value;
                mSr.alpha = c.alpha;
            }
            //else if (mMat != null)
            //{
            //    Color c = mMat.color;
            //    c.a = value;
            //    mMat.color = c;
            //}
            //else if (mText != null)
            //{
            //    Color c = mText.color;
            //    c.a = value;
            //    mText.color = c;
            //}
        }
    }

    /// <summary>
    /// Tween the value.
    /// </summary>

    protected override void OnUpdate(float factor, bool isFinished) { value = Mathf.Lerp(from, to, factor); }

    /// <summary>
    /// Start the tweening operation.
    /// </summary>

    static public TweenCanvasGroupAlpha Begin(GameObject go, float duration, float alpha)
    {
        TweenCanvasGroupAlpha comp = UITweener.Begin<TweenCanvasGroupAlpha>(go, duration);
        comp.from = comp.value;
        comp.to = alpha;

        if (duration <= 0f)
        {
            comp.Sample(1f, true);
            comp.enabled = false;
        }
        return comp;
    }

    public override void SetStartToCurrentValue() { from = value; }
    public override void SetEndToCurrentValue() { to = value; }
}
