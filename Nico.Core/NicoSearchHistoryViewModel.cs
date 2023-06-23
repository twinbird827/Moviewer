﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TBird.Wpf;

namespace Moviewer.Nico.Core
{
    public class NicoSearchHistoryViewModel : BindableBase
    {
        public NicoSearchHistoryViewModel(INicoSearchHistoryParentViewModel parent, NicoSearchHistoryModel m)
        {
            Parent = parent;

            m.AddOnPropertyChanged(this, (sender, e) =>
            {
                Word = m.Word;
            }, nameof(Word), true);

            m.AddOnPropertyChanged(this, (sender, e) =>
            {
                Type = m.Type;
                Display = GetDisplay();
                UserInfo = Display as NicoUserViewModel;
            }, nameof(Type), true);

            m.AddOnPropertyChanged(this, (sender, e) =>
            {
                Date = m.Date;
            }, nameof(Date), true);
        }

        public INicoSearchHistoryParentViewModel Parent { get; private set; }

        public string Word
        {
            get => _Word;
            set => SetProperty(ref _Word, value);
        }
        private string _Word;

        public NicoSearchType Type
        {
            get => _Type;
            set => SetProperty(ref _Type, value);
        }
        private NicoSearchType _Type;

        public DateTime Date
        {
            get => _Date;
            set => SetProperty(ref _Date, value);
        }
        private DateTime _Date;

        public object Display
        {
            get => _Display;
            set => SetProperty(ref _Display, value);
        }
        private object _Display;

        public NicoUserViewModel UserInfo
        {
            get => _UserInfo;
            set => SetProperty(ref _UserInfo, value);
        }
        private NicoUserViewModel _UserInfo;

        private object GetDisplay()
        {
            switch (Type)
            {
                case NicoSearchType.User:
                    return new NicoUserViewModel(new NicoUserModel(Word, null));
                default:
                    return Word;
            }
        }

        public ICommand OnDelete => _OnDelete = _OnDelete ?? RelayCommand.Create(_ =>
        {
            NicoModel.DelSearchHistory(Word, Type);
        });
        private ICommand _OnDelete;

        public ICommand OnDoubleClick => _OnDoubleClick = _OnDoubleClick ?? RelayCommand.Create(_ =>
        {
            Parent.NicoSearchHistoryDoubleClick(this);
        });
        private ICommand _OnDoubleClick;

        public ICommand OnKeyDown => _OnKeyDown = _OnKeyDown ?? RelayCommand.Create<KeyEventArgs>(e =>
        {
            if (e.Key == Key.Enter)
            {
                OnDoubleClick.Execute(null);
            }
        });
        private ICommand _OnKeyDown;

        public ICommand OnFavoriteAdd => _OnFavoriteAdd = _OnFavoriteAdd ?? RelayCommand.Create(_ =>
        {
            NicoModel.AddSearchFavorite(Word, Type);
        });
        private ICommand _OnFavoriteAdd;

        public ICommand OnFavoriteDel => _OnFavoriteDel = _OnFavoriteDel ?? RelayCommand.Create(_ =>
        {
            NicoModel.DelSearchFavorite(Word, Type);
        });
        private ICommand _OnFavoriteDel;

    }
}
