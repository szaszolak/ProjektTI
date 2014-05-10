/*
The MIT License (MIT)
Copyright (c) 2012 Denys Vuika

Permission is hereby granted, free of charge, to any person obtaining a copy of this software 
and associated documentation files (the "Software"), to deal in the Software without restriction, 
including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, 
and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, 
subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. 
IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, 
DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, 
ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ShapeConnectors
{
  public partial class Window1 : Window
  {
    // simple flag for enabling "New thumb" mode
    private bool _isAddNew;

    // Paths for our predefined thumbs
    private Path _path1;
    private Path _path2;
    private Path _path3;
    private Path _path4;

    public List<MyThumb> Vertex = new List<MyThumb>();

    Random rand = new Random();

    private void AddVortex(String MAC, double x, double y)
    {
        var newThumb = new MyThumb(MAC);
        Vertex.Add(newThumb);

        newThumb.Template = Resources["template1"] as ControlTemplate;
        newThumb.ApplyTemplate();
        newThumb.DragDelta += OnDragDelta;
        myCanvas.Children.Add(newThumb);

        var img = (Image)newThumb.Template.FindName("tplImage", newThumb);
        img.Source = new BitmapImage(new Uri("Images/pc.png", UriKind.Relative));

        var txt = (TextBlock)newThumb.Template.FindName("tplTextBlock", newThumb);
        txt.Text = MAC;

        Canvas.SetLeft(newThumb, x);
        Canvas.SetTop(newThumb, y);
        Panel.SetZIndex(newThumb, 1);
        newThumb.UpdateLayout();

        Mouse.OverrideCursor = null;

    }

    private void AddRouter(String MAC, double x, double y)
    {
        var newThumb = new MyThumb(MAC);
        Vertex.Add(newThumb);

        newThumb.Template = Resources["template1"] as ControlTemplate;
        newThumb.ApplyTemplate();
        newThumb.DragDelta += OnDragDelta;
        myCanvas.Children.Add(newThumb);

        var img = (Image)newThumb.Template.FindName("tplImage", newThumb);
        img.Source = new BitmapImage(new Uri("Images/router.png", UriKind.Relative));

        var txt = (TextBlock)newThumb.Template.FindName("tplTextBlock", newThumb);
        txt.Text = MAC;

        Canvas.SetLeft(newThumb, x);
        Canvas.SetTop(newThumb, y);
        Panel.SetZIndex(newThumb, 1);
        newThumb.UpdateLayout();

        Mouse.OverrideCursor = null;

    }


    private void AddEdge(String source, String destiny, int weight)
    {
        var newPath = new Path { Stroke = Brushes.Black, StrokeThickness = weight };
        myCanvas.Children.Add(newPath);

        var newLine = new LineGeometry();
        newPath.Data = newLine;

        foreach (MyThumb thumb in Vertex)
        {
            if (thumb.name == source)
            {
                thumb.StartLines.Add(newLine);
                UpdateLines(thumb);
            }
            if (thumb.name == destiny)
            {
                thumb.EndLines.Add(newLine);
                UpdateLines(thumb);
            }

        }

    }

    public List<String> filePaths = new List<string>();

    List<NetworkBoundModel> wykrytePolaczenia;
    public Window1()
    {
        InitializeComponent();
            

            
              List<String> filePaths = new List<string>();
              filePaths.Add("C:\\polaczone.pcap");
              FileManager tmp1 = new FileManager(filePaths);
                tmp1.start_analizis();
                
                NetworMapper mapowatorSieci = new NetworMapper(tmp1.CapturedPackets);
                mapowatorSieci.mapNetwork();
                wykrytePolaczenia = mapowatorSieci.Bounds;

                Random rand = new Random();

                foreach (NetworkBoundModel model in wykrytePolaczenia)
                {
                    if (!Vertex.Exists(x => x.name == model.boundedNodes[0]))
                    {
             
                        AddVortex(model.boundedNodes[0], rand.Next(100, 300), rand.Next(100, 300));
                    }
                    if (!Vertex.Exists(x => x.name == model.boundedNodes[1]))
                    {
                  
                        AddVortex(model.boundedNodes[1], rand.Next(100, 300), rand.Next(100, 300));
                    }
                }

            foreach(NetworkBoundModel model in wykrytePolaczenia)
            {
                pole.Text += "\n";
                pole.Text += model.boundedNodes[0];
                
            }
            





    }
       



    


    // Event hanlder for dragging functionality support
    private void OnDragDelta(object sender, DragDeltaEventArgs e)
    {
      var thumb = e.Source as MyThumb;

      var left = Canvas.GetLeft(thumb) + e.HorizontalChange;
      var top = Canvas.GetTop(thumb) + e.VerticalChange;

      Canvas.SetLeft(thumb, left);
      Canvas.SetTop(thumb, top);

      // Update lines's layouts
      UpdateLines(thumb);
    }

    // This method updates all the starting and ending lines assigned for the given thumb 
    // according to the latest known thumb position on the canvas
    private static void UpdateLines(MyThumb thumb)
    {
      var left = Canvas.GetLeft(thumb);
      var top = Canvas.GetTop(thumb);

      foreach (var line in thumb.StartLines)
        line.StartPoint = new Point(left + thumb.ActualWidth / 2, top + thumb.ActualHeight / 2);

      foreach (var line in thumb.EndLines)
        line.EndPoint = new Point(left + thumb.ActualWidth / 2, top + thumb.ActualHeight / 2);
    }

    private void WindowLoaded(object sender, RoutedEventArgs e)
    {
      // Move all the predefined thumbs to the front to be over the lines


      #region Initialize paths for predefined thumbs

      _path1 = new Path { Stroke = Brushes.Black, StrokeThickness = 1 };
      _path2 = new Path { Stroke = Brushes.Blue, StrokeThickness = 1 };
      _path3 = new Path { Stroke = Brushes.Green, StrokeThickness = 1 };
      _path4 = new Path { Stroke = Brushes.Red, StrokeThickness = 1 };

      myCanvas.Children.Add(_path1);
      myCanvas.Children.Add(_path2);
      myCanvas.Children.Add(_path3);
      myCanvas.Children.Add(_path4);

      #endregion

      #region Initialize line geometry for predefined thumbs

      var line1 = new LineGeometry();
      _path1.Data = line1;

      var line2 = new LineGeometry();
      _path2.Data = line2;

      var line3 = new LineGeometry();
      _path3.Data = line3;

      var line4 = new LineGeometry();
      _path4.Data = line4;

      #endregion

      #region Setup connections for predefined thumbs

      #endregion

      #region Update lines' layouts


      #endregion

      PreviewMouseLeftButtonDown += Window1PreviewMouseLeftButtonDown;
    }

    // Event handler for creating new thumb element by left mouse click
    // and visually connecting it to the myThumb2 element
    private void Window1PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      if (!_isAddNew) return;

      // Create new thumb object
      var newThumb = new MyThumb("example");
      // Assign our custom template to it
      newThumb.Template = Resources["template1"] as ControlTemplate;
      // Calling ApplyTemplate enables us to navigate the visual tree right now (important!)
      newThumb.ApplyTemplate();
      // Add the "onDragDelta" event handler that is common to all objects
      newThumb.DragDelta += OnDragDelta;
      // Put newly created thumb on the canvas
      myCanvas.Children.Add(newThumb);

      // Access the image element of our custom template and assign it to the new image
      var img = (Image)newThumb.Template.FindName("tplImage", newThumb);
      img.Source = new BitmapImage(new Uri("Images/router.png", UriKind.Relative));

      // Access the textblock element of template and change it too
      var txt = (TextBlock)newThumb.Template.FindName("tplTextBlock", newThumb);
      txt.Text = "System action";

      // Set the position of the object according to the mouse pointer                
      var position = e.GetPosition(this);
      Canvas.SetLeft(newThumb, position.X);
      Canvas.SetTop(newThumb, position.Y);
      // Move our thumb to the front to be over the lines
      Panel.SetZIndex(newThumb, 1);
      // Manually update the layout of the thumb (important!)
      newThumb.UpdateLayout();

      // Create new path and put it on the canvas
      var newPath = new Path { Stroke = Brushes.Black, StrokeThickness = 1 };
      myCanvas.Children.Add(newPath);

      // Create new line geometry element and assign the path to it
      var newLine = new LineGeometry();
      newPath.Data = newLine;

      // Predefined "myThumb2" element will host the starting point

      // Our new thumb will host the ending point of the line



      _isAddNew = false;
      Mouse.OverrideCursor = null;
      btnNewAction.IsEnabled = true;
      e.Handled = true;
    }

    // Event handler for enabling new thumb creation by left mouse button click
    private void BtnNewActionClick(object sender, RoutedEventArgs e)
    {
      _isAddNew = true;
      Mouse.OverrideCursor = Cursors.SizeAll;
      btnNewAction.IsEnabled = false;
    }

    private void addVertexClick(object sender, RoutedEventArgs e)
    {
        Random rand = new Random();/*
     foreach(MyThumb thumb in Vertex)
     {
         AddVortex(thumb.name, rand.Next(100,200), rand.Next(100,200));
     }
        */
        //AddVortex(Vertex[0].name, 60, 60);
        //AddVortex(Vertex[1].name, 80, 110);


        pole.Text += "\n" + Vertex.Count;
        //AddVortex(address.Text, rand.Next(50, 300), rand.Next(50, 300));
    }

    private void addRouterClick(object sender, RoutedEventArgs e)
    {
        AddRouter(address.Text, rand.Next(50, 300), rand.Next(50, 300));
    }

    private void addEdgeClick(object sender, RoutedEventArgs e)
    {
        //AddEdge(source.Text, destiny.Text, 1);
        
        foreach (NetworkBoundModel bound in wykrytePolaczenia)
        {
            AddEdge(bound.boundedNodes[0], bound.boundedNodes[1], 1);
        }
    }
  }
}