using Prism.Mvvm;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.ComponentModel;
using System.Reactive.Disposables;
using System.Windows;

namespace Image_DragAndDrop.ViewModels
{
	public class MainWindowViewModel : BindableBase, INotifyPropertyChanged
	{
		/// <summary>
		/// ウィンドウに表示するタイトル
		/// </summary>
		public ReactivePropertySlim<string> Title { get; } = new ReactivePropertySlim<string>("画像をドラッグ＆ドロップして表示");

		/// <summary>
		/// MainWindowのCloseイベント
		/// </summary>
		public ReactiveCommand ClosedCommand { get; } = new ReactiveCommand();

		/// <summary>
		/// Disposeが必要な処理をまとめてやる
		/// </summary>
		private CompositeDisposable Disposable { get; } = new CompositeDisposable();

		/// <summary>
		/// 表示するイメージのファイル名
		/// </summary>
		public ReactivePropertySlim<string> ViewImage { get; } = new ReactivePropertySlim<string>();

		/// <summary>
		/// ImageのPreviewDragOverイベントのコマンド
		/// </summary>
		public ReactiveCommand<DragEventArgs> PreviewDragOverCommand { get; } = new ReactiveCommand<DragEventArgs>();

		/// <summary>
		/// Imageのイベントのコマンド
		/// </summary>
		public ReactiveCommand<DragEventArgs> DropCommand { get; } = new ReactiveCommand<DragEventArgs>();

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public MainWindowViewModel()
		{
			ViewImage.Value = @"\Image_DragAndDrop;component\Resource\blank.jpg";
			PreviewDragOverCommand.Subscribe(ImagePreviewDragOver).AddTo(Disposable);
			DropCommand.Subscribe(ImageDrop).AddTo(Disposable);
			ClosedCommand.Subscribe(Close).AddTo(Disposable);
		}

		/// <summary>
		/// ウィンドウが閉じられるイベント
		/// </summary>
		private void Close()
		{
			Disposable.Dispose();
		}

		/// <summary>
		/// ImageのPreviewDragOverイベントに対する処理
		/// </summary>
		/// <param name="e"></param>
		private void ImagePreviewDragOver(DragEventArgs e)
		{
			// マウスカーソルをコピーにする。
			e.Effects = DragDropEffects.Copy;
			// ドラッグされてきたものがFileDrop形式の場合だけ、このイベントを処理済みにする。
			e.Handled = e.Data.GetDataPresent(DataFormats.FileDrop);
		}

		/// <summary>
		/// ImageのDropイベントに対する処理
		/// </summary>
		/// <param name="e"></param>
		private void ImageDrop(DragEventArgs e)
		{
			// ドロップされたものがFileDrop形式の場合は、各ファイルのパス文字列を文字列配列に格納する。
			string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
			// 複数ドロップの可能性もあるので、今回は最初のファイルを選択して表示
			ViewImage.Value = files[0];
		}
	}
}
