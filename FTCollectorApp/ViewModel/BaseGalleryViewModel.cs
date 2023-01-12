﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Windows.Input;
using FTCollectorApp.Model;
using Xamarin.CommunityToolkit.ObjectModel;

namespace FTCollectorApp.ViewModel
{
	public abstract class BaseGalleryViewModel : BaseViewModel
	{
		public BaseGalleryViewModel()
		{
			Items = CreateItems().OrderBy(x => x.Title).ToList();
			Filter();
			FilterCommand = CommandFactory.Create(Filter);
		}

		public IReadOnlyList<SectionModel> Items { get; }

		public ICommand FilterCommand { get; }

		public string FilterValue { private get; set; } = string.Empty;

		public IEnumerable<SectionModel> FilteredItems { get; private set; } = Enumerable.Empty<SectionModel>();

		protected abstract IEnumerable<SectionModel> CreateItems();

		void Filter()
		{
			FilterValue ??= string.Empty;
			FilteredItems = Items.Where(item => item.Title.IndexOf(FilterValue, StringComparison.InvariantCultureIgnoreCase) >= 0);
			OnPropertyChanged(nameof(FilteredItems));
		}
	}
}