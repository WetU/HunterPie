﻿using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace HunterPie.UI.Architecture.Converters;

public class BorderClipConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values.Length != 4 || values[0] is not double width || values[1] is not double height ||
            values[2] is not CornerRadius radius || values[3] is not Thickness borderThickness)
            return DependencyProperty.UnsetValue;

        if (width < double.Epsilon || height < double.Epsilon)
            return Geometry.Empty;

        var clip = new RectangleGeometry(
            new Rect(0, 0, width, height),
            radius.TopLeft + borderThickness.Top,
            radius.TopLeft + borderThickness.Left
        );
        clip.Freeze();

        return clip;

    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}