using System;
using AndroidX.RecyclerView.Widget;
using MvvmCross.AdvancedRecyclerView.ViewHolders;

namespace MvvmCross.AdvancedRecyclerView.Data.EventArguments
{
    public class MvxSwipeBackgroundSetEventArgs
    {
        public MvxSwipeBackgroundSetEventArgs()
        {
        }

        public MvxAdvancedRecyclerViewHolder ViewHolder { get; internal set; }

        public virtual int Position { get; internal set; }

        public int Type { get; internal set; }
    }
}
