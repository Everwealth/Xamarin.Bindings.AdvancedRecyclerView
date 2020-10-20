using Android.Views;
using AndroidX.Core.View;

namespace MvvmCross.AdvancedRecyclerView.Extensions
{
    public static class ViewUtils
    {
        public static bool HitTest(this View v, int x, int y)
        {
            var tx = (int) (ViewCompat.GetTranslationX(v) + 0.5f);
            var ty = (int) (ViewCompat.GetTranslationY(v) + 0.5f);
            var left = v.Left + tx;
            var right = v.Right + tx;
            var top = v.Top + ty;
            var bottom = v.Bottom + ty;

            return x >= left && x <= right && y >= top && y <= bottom;
        }
    }
}