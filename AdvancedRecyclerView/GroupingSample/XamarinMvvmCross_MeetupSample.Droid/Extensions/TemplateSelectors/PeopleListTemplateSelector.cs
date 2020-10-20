using System;
using MvvmCross.AdvancedRecyclerView.TemplateSelectors;
using XamarinMvvmCross_MeetupSample.Core;

namespace XamarinMvvmCross_MeetupSample.Droid
{
    public class PeopleListTemplateSelector : MvxExpandableTemplateSelector, IMvxHeaderTemplate, IMvxFooterTemplate
	{
        public PeopleListTemplateSelector() : base(Resource.Layout.person_group_header)
        {
        }

        public int HeaderLayoutId { get; set; }
        public int FooterLayoutId { get; set; }

        protected override int GetChildItemLayoutId(int fromViewType)
        {
		    return fromViewType == 1 ? Resource.Layout.special_person_layout : Resource.Layout.person_layout;
		}

        protected override int GetChildItemViewType(object forItemObject)
        {
            var person = forItemObject as Person;

            return person.IsSpecialPerson ? 1 : 2;
        } 
    }
}
