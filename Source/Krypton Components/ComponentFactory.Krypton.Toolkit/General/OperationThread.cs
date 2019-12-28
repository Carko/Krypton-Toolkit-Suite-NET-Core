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

using System;

namespace ComponentFactory.Krypton.Toolkit
{
    internal class OperationThread : GlobalId
    {
        #region Instance Fields
        private readonly Operation _op;
        private readonly object _parameter;
        private int _state;

        #endregion

        #region Identity
        /// <summary>
        /// Initialize a new instance of the OperationThread class.
        /// </summary>
        /// <param name="op">Operation to perform on thread.</param>
        /// <param name="parameter">Parameter to pass into operation.</param>
        public OperationThread(Operation op, object parameter)
        {
            // Remember the passed operation details
            _op = op;
            _parameter = parameter;

            // Operation still running
            _state = 0;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the current operation state.
        /// </summary>
        public int State
        {
            get 
            {
                int ret;

                // Read the state in a thread state way
                lock (this)
                {
                    ret = _state;
                }

                return ret; 
            }
        }

        /// <summary>
        /// Gets the result from the operation.
        /// </summary>
        public object Result { get; private set; }

        /// <summary>
        /// Gets the exception generated by operation.
        /// </summary>
        public Exception Exception { get; private set; }

        #endregion

        #region Run
        /// <summary>
        /// Entry point for performing operation.
        /// </summary>
        public void Run()
        {
            try
            {
                // Execute the operation
                Result = _op(_parameter);

                // Success
                lock(this)
                {
                    _state = 1;
                }
            }
            catch(Exception ex)
            {
                // Remember the exception details
                Exception = ex;

                // Failed with exception
                lock(this)
                {
                    _state = 2;
                }
            }
        }
        #endregion
    }
}
