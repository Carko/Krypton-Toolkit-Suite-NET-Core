﻿// *****************************************************************************
// BSD 3-Clause License (https://github.com/ComponentFactory/Krypton/blob/master/LICENSE)
//  © Component Factory Pty Ltd, 2006-2019, All rights reserved.
// The software and associated documentation supplied hereunder are the 
//  proprietary information of Component Factory Pty Ltd, 13 Swallows Close, 
//  Mornington, Vic 3931, Australia and are supplied subject to license terms.
// 
//  Modifications by Peter Wagner(aka Wagnerp) & Simon Coghlan(aka Smurf-IV) 2017 - 2019. All rights reserved. (https://github.com/Wagnerp/Krypton-NET-5.490)
//  Version 5.490.0.0  www.ComponentFactory.com
// *****************************************************************************

using System.ComponentModel.Design;

namespace ComponentFactory.Krypton.Toolkit
{
    internal class KryptonWrapLabelActionList : DesignerActionList
    {
        #region Instance Fields
        private readonly KryptonWrapLabel _wrapLabel;
        private readonly IComponentChangeService _service;
        #endregion

        #region Identity
        /// <summary>
        /// Initialize a new instance of the KryptonWrapLabelActionList class.
        /// </summary>
        /// <param name="owner">Designer that owns this action list instance.</param>
        public KryptonWrapLabelActionList(KryptonWrapLabelDesigner owner)
            : base(owner.Component)
        {
            // Remember the label instance
            _wrapLabel = owner.Component as KryptonWrapLabel;

            // Cache service used to notify when a property has changed
            _service = (IComponentChangeService)GetService(typeof(IComponentChangeService));
        }
        #endregion
        
        #region Public
        /// <summary>
        /// Gets and sets the label style.
        /// </summary>
        public LabelStyle LabelStyle
        {
            get => _wrapLabel.LabelStyle;

            set
            {
                if (_wrapLabel.LabelStyle != value)
                {
                    _service.OnComponentChanged(_wrapLabel, null, _wrapLabel.LabelStyle, value);
                    _wrapLabel.LabelStyle = value;
                }
            }
        }

        /// <summary>
        /// Gets and sets the palette mode.
        /// </summary>
        public PaletteMode PaletteMode
        {
            get => _wrapLabel.PaletteMode;

            set
            {
                if (_wrapLabel.PaletteMode != value)
                {
                    _service.OnComponentChanged(_wrapLabel, null, _wrapLabel.PaletteMode, value);
                    _wrapLabel.PaletteMode = value;
                }
            }
        }
        #endregion

        #region Public Override
        /// <summary>
        /// Returns the collection of DesignerActionItem objects contained in the list.
        /// </summary>
        /// <returns>A DesignerActionItem array that contains the items in this list.</returns>
        public override DesignerActionItemCollection GetSortedActionItems()
        {
            // Create a new collection for holding the single item we want to create
            DesignerActionItemCollection actions = new DesignerActionItemCollection();

            // This can be null when deleting a control instance at design time
            if (_wrapLabel != null)
            {
                actions.Add(new DesignerActionHeaderItem("Appearance"));
                actions.Add(new DesignerActionPropertyItem("LabelStyle", "Style", "Appearance", "Label style"));
                actions.Add(new DesignerActionHeaderItem("Visuals"));
                actions.Add(new DesignerActionPropertyItem("PaletteMode", "Palette", "Visuals", "Palette applied to drawing"));
            }
            
            return actions;
        }
        #endregion
    }
}
