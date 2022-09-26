﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using TB = System.Windows.Controls.TextBox;

namespace HunterPie.UI.Controls.Sliders
{
    /// <summary>
    /// Interaction logic for Range.xaml
    /// </summary>
    public partial class Range : UserControl
    {

        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(double), typeof(Range), new PropertyMetadata(0.0));
        
        public double Maximum
        {
            get { return (double)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }
        public static readonly DependencyProperty MaximumProperty =
            DependencyProperty.Register("Maximum", typeof(double), typeof(Range), new PropertyMetadata(0.0));

        public double Minimum
        {
            get { return (double)GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }
        public static readonly DependencyProperty MinimumProperty =
            DependencyProperty.Register("Minimum", typeof(double), typeof(Range), new PropertyMetadata(0.0));

        public double Change
        {
            get { return (double)GetValue(ChangeProperty); }
            set { SetValue(ChangeProperty, value); }
        }
        public static readonly DependencyProperty ChangeProperty =
            DependencyProperty.Register("Change", typeof(double), typeof(Range), new PropertyMetadata(1.0));

        public Range()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && sender is TB textbox)
                UpdateBinding(textbox);

        }

        private void OnLostFocus(object sender, RoutedEventArgs e) => UpdateBinding(sender as TB);

        private static void UpdateBinding(TB textbox)
        {
            BindingExpression binding = textbox.GetBindingExpression(TB.TextProperty);
            binding.UpdateSource();
        }
    }
}
