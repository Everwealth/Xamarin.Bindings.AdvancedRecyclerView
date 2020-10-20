using System;
using Android.Content;
using Android.Content.Res;
using Android.Util;
using MvvmCross.AdvancedRecyclerView.Data;
using MvvmCross.AdvancedRecyclerView.Data.ItemUniqueIdProvider;
using MvvmCross.AdvancedRecyclerView.TemplateSelectors;
using MvvmCross.AdvancedRecyclerView.Utils;
using MvvmCross.DroidX.RecyclerView.ItemTemplates;
using MvvmCross.Logging;
using MvvmCross.Platforms.Android.Binding.ResourceHelpers;
using MvvmCross.Platforms.Android.Binding.Views;

namespace MvvmCross.AdvancedRecyclerView.Extensions
{
    static class MvxAdvancedRecyclerViewAttributeExtensions
    {
        static MvxAdvancedRecyclerViewAttributes ReadRecyclerViewItemTemplateSelectorAttributes(Context context, IAttributeSet attrs)
        {
            TryInitializeBindingResourcePaths();

            TypedArray typedArray = null;

            string templateSelectorClassName = string.Empty;
            string groupedDataConverterClassName = string.Empty;
            string groupExpandControllerClassName = string.Empty;
            string swipeableTemplateClassName = string.Empty;
            string uniqueItemIdProviderClassName = string.Empty;
            string groupSwipeableTemplateClassName = string.Empty;
            string childSwipeableTemplateClassName = string.Empty;
            
            int headerLayoutId = 0;
            int footerLayoutId = 0;

            try
            {
                typedArray = context.ObtainStyledAttributes(attrs, MvxRecyclerViewGroupId);
                int numberOfStyles = typedArray.IndexCount;

                for (int i = 0; i < numberOfStyles; ++i)
                {
                    var attributeId = typedArray.GetIndex(i);

                    if (attributeId == MvxRecyclerViewGroupExpandController)
                        groupExpandControllerClassName = typedArray.GetString(attributeId);
                    if (attributeId == MvxRecyclerViewItemTemplateSelector)
                        templateSelectorClassName = typedArray.GetString(attributeId);
                    if (attributeId == MvxRecyclerViewHeaderLayoutId)
                        headerLayoutId = typedArray.GetResourceId(attributeId, 0);
                    if (attributeId == MvxRecyclerViewFooterLayoutId)
                        footerLayoutId = typedArray.GetResourceId(attributeId, 0);
                    if (attributeId == MvxRecyclerViewGroupedDataConverter)
                        groupedDataConverterClassName = typedArray.GetString(attributeId);
                    if (attributeId == MvxRecyclerViewSwipeableTemplate)
                        swipeableTemplateClassName = typedArray.GetString(attributeId);
                    if (attributeId == MvxRecyclerViewUniqueItemIdProvider)
                        uniqueItemIdProviderClassName = typedArray.GetString(attributeId);
                    if (attributeId == MvxRecyclerViewGroupSwipeableTemplate)
                        groupSwipeableTemplateClassName = typedArray.GetString(attributeId);
                    if (attributeId == MvxRecyclerViewChildSwipeableTemplate)
                        childSwipeableTemplateClassName = typedArray.GetString(attributeId);
                }
            }
            finally
            {
                typedArray?.Recycle();
            }

            if (string.IsNullOrEmpty(templateSelectorClassName))
                templateSelectorClassName = typeof(MvxDefaultTemplateSelector).FullName;
            if (string.IsNullOrEmpty(groupExpandControllerClassName))
                groupExpandControllerClassName = typeof(DefaultMvxGroupExpandController).FullName;

            return new MvxAdvancedRecyclerViewAttributes()
            {
                TemplateSelectorClassName = templateSelectorClassName,
                ItemTemplateLayoutId = MvxAttributeHelpers.ReadListItemTemplateId(context, attrs),
                FooterLayoutId = footerLayoutId,
                HeaderLayoutId = headerLayoutId,
                GroupedDataConverterClassName = groupedDataConverterClassName,
                GroupExpandControllerClassName = groupExpandControllerClassName,
                SwipeableTemplateClassName = swipeableTemplateClassName,
                UniqueItemIdProviderClassName = uniqueItemIdProviderClassName,
                GroupSwipeableTemplateClassName = groupSwipeableTemplateClassName,
                ChildSwipeableTemplateClassName = childSwipeableTemplateClassName
            };
        }

        public static bool IsGroupingSupported(Context context, IAttributeSet attrs)
            => !string.IsNullOrEmpty(ReadRecyclerViewItemTemplateSelectorAttributes(context, attrs).GroupedDataConverterClassName);

        public static bool IsSwipeSupported(Context context, IAttributeSet attrs)
            => !string.IsNullOrEmpty(ReadRecyclerViewItemTemplateSelectorAttributes(context, attrs).SwipeableTemplateClassName);

        public static bool IsSwipeForExpandableSupported(Context context, IAttributeSet attrs)
            => IsGroupedSwipeSupported(context, attrs) || IsGroupedChildSwipeSupported(context, attrs);

        public static bool IsGroupedSwipeSupported(Context context, IAttributeSet attrs)
            => !string.IsNullOrEmpty(ReadRecyclerViewItemTemplateSelectorAttributes(context, attrs).GroupSwipeableTemplateClassName);

        public static bool IsGroupedChildSwipeSupported(Context context, IAttributeSet attrs)
            => !string.IsNullOrEmpty(ReadRecyclerViewItemTemplateSelectorAttributes(context, attrs).ChildSwipeableTemplateClassName);

        public static MvxExpandableDataConverter BuildMvxGroupedDataConverter(Context context, IAttributeSet attrs)
        {
            var groupedDataConverterClassName = ReadRecyclerViewItemTemplateSelectorAttributes(context, attrs).GroupedDataConverterClassName;
            var type = Type.GetType(groupedDataConverterClassName);

            if (type == null)
            {
                var message = $"Can't build Grouped Data Converter." +
                    $"Sorry but type with class name: {groupedDataConverterClassName} does not exist." +
                              $"Make sure you have provided full Type name: namespace + class name, AssemblyName.";
                Mvx.Resolve<IMvxLog>().Log(MvxLogLevel.Error, () => message); 
                throw new InvalidOperationException(message);
            }

            if (!typeof(MvxExpandableDataConverter).IsAssignableFrom(type))
            {
                string message = $"Sorry but type: {type} does not implement {nameof(MvxExpandableDataConverter)} interface.";
                Mvx.Resolve<IMvxLog>().Log(MvxLogLevel.Error, () => message);
                throw new InvalidOperationException(message);
            }

            if (type.IsAbstract)
            {
                string message = $"Sorry can not instatiate {nameof(MvxExpandableDataConverter)} as provided type: {type} is abstract/interface.";
                Mvx.Resolve<IMvxLog>().Log(MvxLogLevel.Error, () => message);
                throw new InvalidOperationException(message);
            }

            return Activator.CreateInstance(type) as MvxExpandableDataConverter;
        }

        public static IMvxTemplateSelector BuildItemTemplateSelector(Context context, IAttributeSet attrs)
        {
            var templateSelectorAttributes = ReadRecyclerViewItemTemplateSelectorAttributes(context, attrs);
            var type = Type.GetType(templateSelectorAttributes.TemplateSelectorClassName);

            if (type == null && templateSelectorAttributes.ItemTemplateLayoutId == 0)
            {
                var message = $"Cant create template selector." +
                    $"Sorry but type with class name: {templateSelectorAttributes.TemplateSelectorClassName} does not exist." +
                             $"Make sure you have provided full Type name: namespace + class name, AssemblyName." +
                              $"Example (check Example.Droid sample!): Example.Droid.Common.TemplateSelectors.MultiItemTemplateModelTemplateSelector, Example.Droid";
                Mvx.Resolve<IMvxLog>().Log(MvxLogLevel.Error, () => message);
                throw new InvalidOperationException(message);
            }

            if (type != null && !typeof(IMvxTemplateSelector).IsAssignableFrom(type))
            {
                string message = $"Sorry but type: {type} does not implement {nameof(IMvxTemplateSelector)} interface.";
                Mvx.Resolve<IMvxLog>().Log(MvxLogLevel.Error, () => message);
                throw new InvalidOperationException(message);
            }

            if (type?.IsAbstract ?? false)
            {
                string message = $"Sorry can not instatiate {nameof(IMvxTemplateSelector)} as provided type: {type} is abstract/interface.";
                Mvx.Resolve<IMvxLog>().Log(MvxLogLevel.Error, () => message);
                throw new InvalidOperationException(message);
            }

            IMvxTemplateSelector templateSelector = null;
            if (type != null)
                templateSelector = Activator.CreateInstance(type) as IMvxTemplateSelector;
            else
                templateSelector = new MvxDefaultHeaderFooterTemplateSelector(templateSelectorAttributes.ItemTemplateLayoutId);
            
            var headerTemplate = templateSelector as IMvxHeaderTemplate;
            var footerTemplate = templateSelector as IMvxFooterTemplate;

            if (headerTemplate != null)
                headerTemplate.HeaderLayoutId = templateSelectorAttributes.HeaderLayoutId;

            if (footerTemplate != null)
                footerTemplate.FooterLayoutId = templateSelectorAttributes.FooterLayoutId;

            return templateSelector;
        }

        public static MvxGroupExpandController BuildGroupExpandController(Context context, IAttributeSet attrs)
        {
            var groupExpandControllerClassName = ReadRecyclerViewItemTemplateSelectorAttributes(context, attrs).GroupExpandControllerClassName;
            var type = Type.GetType(groupExpandControllerClassName);

            if (type == null)
            {
                var message = $"Can't build GroupExpandController." +
                    $"Sorry but type with class name: {groupExpandControllerClassName} does not exist." +
                              $"Make sure you have provided full Type name: namespace + class name, AssemblyName.";
                Mvx.Resolve<IMvxLog>().Log(MvxLogLevel.Error, () => message);
                throw new InvalidOperationException(message);
            }

            if (!typeof(MvxGroupExpandController).IsAssignableFrom(type))
            {
                string message = $"Sorry but type: {type} does not implement {nameof(MvxGroupExpandController)} interface.";
                Mvx.Resolve<IMvxLog>().Log(MvxLogLevel.Error, () => message);
                throw new InvalidOperationException(message);
            }

            if (type.IsAbstract)
            {
                string message = $"Sorry can not instatiate {nameof(MvxGroupExpandController)} as provided type: {type} is abstract/interface.";
                Mvx.Resolve<IMvxLog>().Log(MvxLogLevel.Error, () => message);
                throw new InvalidOperationException(message);
            }

            return Activator.CreateInstance(type) as MvxGroupExpandController;
        }

        public static MvxSwipeableTemplate BuildSwipeableTemplate(Context context, IAttributeSet attrs)
        {
			var templateSelectorAttributes = ReadRecyclerViewItemTemplateSelectorAttributes(context, attrs);
            var type = Type.GetType(templateSelectorAttributes.SwipeableTemplateClassName);

			if (type == null)
			{
				var message = $"Can't build swipeable template." +
                    $"Sorry but type with class name: {templateSelectorAttributes.SwipeableTemplateClassName} does not exist." +
							 $"Make sure you have provided full Type name: namespace + class name, AssemblyName." +
							  $"Example (check Example.Droid sample!): Example.Droid.Common.TemplateSelectors.MultiItemTemplateModelTemplateSelector, Example.Droid";
			    Mvx.Resolve<IMvxLog>().Log(MvxLogLevel.Error, () => message);
			    throw new InvalidOperationException(message);
			}

            if (!typeof(MvxSwipeableTemplate).IsAssignableFrom(type))
			{
                string message = $"Sorry but type: {type} does not implement {nameof(MvxSwipeableTemplate)} interface.";
			    Mvx.Resolve<IMvxLog>().Log(MvxLogLevel.Error, () => message);
			    throw new InvalidOperationException(message);
			}

			if (type.IsAbstract)
			{
                string message = $"Sorry can not instatiate {nameof(MvxSwipeableTemplate)} as provided type: {type} is abstract/interface.";
			    Mvx.Resolve<IMvxLog>().Log(MvxLogLevel.Error, () => message);
			    throw new InvalidOperationException(message);
			}

            var swipeableTemplate = Activator.CreateInstance(type) as MvxSwipeableTemplate;
			return swipeableTemplate;
        }

        public static MvxSwipeableTemplate BuildGroupSwipeableTemplate(Context context, IAttributeSet attrs)
        {
            var templateSelectorAttributes = ReadRecyclerViewItemTemplateSelectorAttributes(context, attrs);
            var type = Type.GetType(templateSelectorAttributes.GroupSwipeableTemplateClassName);

            if (type == null)
            {
                var message = $"Can't build group swipeable template." +
                              $"Sorry but type with class name: {templateSelectorAttributes.GroupSwipeableTemplateClassName} does not exist." +
                              $"Make sure you have provided full Type name: namespace + class name, AssemblyName." +
                              $"Example (check Example.Droid sample!): Example.Droid.Common.TemplateSelectors.MultiItemTemplateModelTemplateSelector, Example.Droid";
                Mvx.Resolve<IMvxLog>().Log(MvxLogLevel.Error, () => message);
                throw new InvalidOperationException(message);
            }

            if (!typeof(MvxSwipeableTemplate).IsAssignableFrom(type))
            {
                string message = $"Sorry but type: {type} does not implement {nameof(MvxSwipeableTemplate)} interface.";
                Mvx.Resolve<IMvxLog>().Log(MvxLogLevel.Error, () => message);
                throw new InvalidOperationException(message);
            }

            if (type.IsAbstract)
            {
                string message = $"Sorry can not instatiate {nameof(MvxSwipeableTemplate)} as provided type: {type} is abstract/interface.";
                Mvx.Resolve<IMvxLog>().Log(MvxLogLevel.Error, () => message);
                throw new InvalidOperationException(message);
            }

            var swipeableTemplate = Activator.CreateInstance(type) as MvxSwipeableTemplate;
            return swipeableTemplate;        }
        
        public static MvxSwipeableTemplate BuildGroupChildSwipeableTemplate(Context context, IAttributeSet attrs)
        {
            var templateSelectorAttributes = ReadRecyclerViewItemTemplateSelectorAttributes(context, attrs);
            var type = Type.GetType(templateSelectorAttributes.ChildSwipeableTemplateClassName);

            if (type == null)
            {
                var message = $"Can't build child swipeable template." +
                              $"Sorry but type with class name: {templateSelectorAttributes.ChildSwipeableTemplateClassName} does not exist." +
                              $"Make sure you have provided full Type name: namespace + class name, AssemblyName." +
                              $"Example (check Example.Droid sample!): Example.Droid.Common.TemplateSelectors.MultiItemTemplateModelTemplateSelector, Example.Droid";
                Mvx.Resolve<IMvxLog>().Log(MvxLogLevel.Error, () => message);
                throw new InvalidOperationException(message);
            }

            if (!typeof(MvxSwipeableTemplate).IsAssignableFrom(type))
            {
                string message = $"Sorry but type: {type} does not implement {nameof(MvxSwipeableTemplate)} interface.";
                Mvx.Resolve<IMvxLog>().Log(MvxLogLevel.Error, () => message);
                throw new InvalidOperationException(message);
            }

            if (type.IsAbstract)
            {
                string message = $"Sorry can not instatiate {nameof(MvxSwipeableTemplate)} as provided type: {type} is abstract/interface.";
                Mvx.Resolve<IMvxLog>().Log(MvxLogLevel.Error, () => message);
                throw new InvalidOperationException(message);
            }

            var swipeableTemplate = Activator.CreateInstance(type) as MvxSwipeableTemplate;
            return swipeableTemplate;
           
        }

        public static IMvxItemUniqueIdProvider BuildUniqueItemIdProvider(Context context, IAttributeSet attrs){
			var templateSelectorAttributes = ReadRecyclerViewItemTemplateSelectorAttributes(context, attrs);
            var type = Type.GetType(templateSelectorAttributes.UniqueItemIdProviderClassName);

			if (type == null)
			{
				var message = $"Can't build unique item id provider." +
                    $"Sorry but type with class name: {templateSelectorAttributes.UniqueItemIdProviderClassName} does not exist." +
							 $"Make sure you have provided full Type name: namespace + class name, AssemblyName." +
							  $"Example (check Example.Droid sample!): Example.Droid.Common.TemplateSelectors.MultiItemTemplateModelTemplateSelector, Example.Droid";
			    Mvx.Resolve<IMvxLog>().Log(MvxLogLevel.Error, () => message);
			    throw new InvalidOperationException(message);
			}

            if (!typeof(IMvxItemUniqueIdProvider).IsAssignableFrom(type))
			{
                string message = $"Sorry but type: {type} does not implement {nameof(IMvxItemUniqueIdProvider)} interface.";
			    Mvx.Resolve<IMvxLog>().Log(MvxLogLevel.Error, () => message);
			    throw new InvalidOperationException(message);
			}

			if (type.IsAbstract)
			{
				string message = $"Sorry can not instatiate {nameof(IMvxItemUniqueIdProvider)} as provided type: {type} is abstract/interface.";
			    Mvx.Resolve<IMvxLog>().Log(MvxLogLevel.Error, () => message);
			    throw new InvalidOperationException(message);
			}

            var uniqueItemIdProvider = Activator.CreateInstance(type) as IMvxItemUniqueIdProvider;
			return uniqueItemIdProvider;
        }

        public static bool IsHidesHeaderIfEmptyEnabled(Context context, IAttributeSet attrs)
        {
            TryInitializeBindingResourcePaths();

            TypedArray typedArray = null;
            bool hidesHeaderIfEmpty = true;

            try
            {
                typedArray = context.ObtainStyledAttributes(attrs, MvxRecyclerViewGroupId);
                int numberOfStyles = typedArray.IndexCount;

                for (int i = 0; i < numberOfStyles; ++i)
                {
                    var attributeId = typedArray.GetIndex(i);

                    if (attributeId == MvxRecyclerViewHidesHeaderIfEmpty)
                    {
                        hidesHeaderIfEmpty = typedArray.GetBoolean(attributeId, true);
                        break;
                    }
                }

                return hidesHeaderIfEmpty;
            }
            finally
            {
                typedArray.Recycle();
            }
        }


        public static bool IsHidesFooterIfEmptyEnabled(Context context, IAttributeSet attrs)
        {
            TryInitializeBindingResourcePaths();

            TypedArray typedArray = null;
            bool hidesFooterIfEmpty = true;

            try
            {
                typedArray = context.ObtainStyledAttributes(attrs, MvxRecyclerViewGroupId);
                int numberOfStyles = typedArray.IndexCount;

                for (int i = 0; i < numberOfStyles; ++i)
                {
                    var attributeId = typedArray.GetIndex(i);

                    if (attributeId == MvxRecyclerViewHidesFooterIfEmpty)
                    {
                        hidesFooterIfEmpty = typedArray.GetBoolean(attributeId, true);
                        break;
                    }
                }

                return hidesFooterIfEmpty;
            }
            finally
            {
                typedArray.Recycle();
            }
        }

        private static bool areBindingResourcesInitialized = false;
        private static void TryInitializeBindingResourcePaths()
        {
            if (areBindingResourcesInitialized)
                return;
            areBindingResourcesInitialized = true;

            var resourceTypeFinder = Mvx.IoCProvider.Resolve<IMvxAppResourceTypeFinder>().Find();
            var styleableType = resourceTypeFinder.GetNestedType("Styleable");

            MvxRecyclerViewGroupId = (int[])styleableType.GetField("MvxRecyclerView").GetValue(null);
            MvxRecyclerViewItemTemplateSelector = (int)styleableType.GetField("MvxRecyclerView_MvxTemplateSelector").GetValue(null);
            MvxRecyclerViewHeaderLayoutId = (int)styleableType.GetField("MvxRecyclerView_MvxHeaderLayoutId").GetValue(null);
            MvxRecyclerViewFooterLayoutId = (int)styleableType.GetField("MvxRecyclerView_MvxFooterLayoutId").GetValue(null);
            MvxRecyclerViewGroupExpandController =
                (int)styleableType.GetField("MvxRecyclerView_MvxGroupExpandController").GetValue(null);
            MvxRecyclerViewGroupedDataConverter =
                (int)styleableType.GetField("MvxRecyclerView_MvxGroupedDataConverter").GetValue(null);
            MvxRecyclerViewHidesHeaderIfEmpty =
                (int)styleableType.GetField("MvxRecyclerView_MvxHidesHeaderIfEmpty").GetValue(null);
            MvxRecyclerViewHidesFooterIfEmpty =
                (int)styleableType.GetField("MvxRecyclerView_MvxHidesFooterIfEmpty").GetValue(null);
            MvxRecyclerViewSwipeableTemplate =
                (int)styleableType.GetField("MvxRecyclerView_MvxSwipeableTemplate").GetValue(null);
            MvxRecyclerViewUniqueItemIdProvider =
                (int)styleableType.GetField("MvxRecyclerView_MvxUniqueItemIdProvider").GetValue(null);
            MvxRecyclerViewGroupSwipeableTemplate =
                (int)styleableType.GetField("MvxRecyclerView_MvxGroupSwipeableTemplate").GetValue(null);
            MvxRecyclerViewChildSwipeableTemplate =
                (int)styleableType.GetField("MvxRecyclerView_MvxChildSwipeableTemplate").GetValue(null);
        }

        private static int[] MvxRecyclerViewGroupId { get; set; }
        private static int MvxRecyclerViewItemTemplateSelector { get; set; }

        private static int MvxRecyclerViewGroupExpandController { get; set; }

        private static int MvxRecyclerViewHeaderLayoutId { get; set; }

        private static int MvxRecyclerViewFooterLayoutId { get; set; }

        private static int MvxRecyclerViewHidesHeaderIfEmpty { get; set; }

        private static int MvxRecyclerViewHidesFooterIfEmpty { get; set; }

        public static int MvxRecyclerViewGroupedDataConverter { get; set; }

        public static int MvxRecyclerViewSwipeableTemplate { get; set; }

        public static int MvxRecyclerViewUniqueItemIdProvider { get; set; }
        
        public static int MvxRecyclerViewGroupSwipeableTemplate { get; set; }
        
        public static int MvxRecyclerViewChildSwipeableTemplate { get; set; }
    }
}
