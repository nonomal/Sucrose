﻿using System.Windows;

namespace Sucrose.XamlAnimatedGif
{
    public delegate void AnimationStartedEventHandler(DependencyObject d, AnimationStartedEventArgs e);

    public class AnimationStartedEventArgs : RoutedEventArgs
    {
        public AnimationStartedEventArgs(object source)
            : base(AnimationBehavior.AnimationStartedEvent, source)
        {
        }
    }
}