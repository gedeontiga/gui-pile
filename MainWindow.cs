using Gtk;
using System;
using System.Collections.Generic;

public class MainWindow : Window
{
    private List<string> dataList;
    private TreeStore treeStore;
    private TreeView treeView;
    private Entry entry;
    private Button addButton;
    private Button updateButton;
    private Button deleteButton;

    public MainWindow() : base("Data App")
    {
        SetDefaultSize(600, 400);
        DeleteEvent += Window_DeleteEvent;

        VBox vbox = new VBox();
        Add(vbox);

        entry = new Entry();
        vbox.PackStart(entry, false, false, 0);

        HBox buttonBox = new HBox();
        vbox.PackStart(buttonBox, false, false, 0);

        addButton = new Button("Add");
        addButton.Clicked += AddButton_Clicked;
        buttonBox.PackStart(addButton, false, false, 0);

        updateButton = new Button("Update");
        updateButton.Clicked += UpdateButton_Clicked;
        buttonBox.PackStart(updateButton, false, false, 0);

        deleteButton = new Button("Delete");
        deleteButton.Clicked += DeleteButton_Clicked;
        buttonBox.PackStart(deleteButton, false, false, 0);

        treeStore = new TreeStore(typeof(string));
        treeView = new TreeView();
        treeView.Model = treeStore;
        treeView.AppendColumn("Data", new CellRendererText(), "text", 0);
        vbox.PackStart(treeView, true, true, 0);

        dataList = new List<string>();
    }

    private void AddButton_Clicked(object sender, EventArgs e)
    {
        string data = entry.Text;
        dataList.Insert(0, data);
        TreeIter iter = treeStore.InsertWithValues(0, data);
        entry.Text = string.Empty;
    }

    private void UpdateButton_Clicked(object sender, EventArgs e)
    {
        TreeSelection selection = treeView.Selection;
        TreeIter iter;
        if (selection.GetSelected(out iter))
        {
            string newData = entry.Text;
            treeStore.SetValue(iter, 0, newData);
            int index = treeStore.GetPath(iter).Indices[0];
            dataList[index] = newData;

            entry.Text = string.Empty;
        }
    }

    private void DeleteButton_Clicked(object sender, EventArgs e)
    {
        TreeSelection selection = treeView.Selection;
        TreeIter iter;
        if (selection.GetSelected(out iter))
        {
            int index = treeStore.GetPath(iter).Indices[0];
            treeStore.Remove(ref iter);
            dataList.RemoveAt(index);

            entry.Text = string.Empty;
        }
    }

    private void Window_DeleteEvent(object sender, DeleteEventArgs a)
    {
        Application.Quit();
        a.RetVal = true;
    }

    public static void Main()
    {
        Application.Init();
        MainWindow win = new MainWindow();
        win.ShowAll();
        Application.Run();
    }
}