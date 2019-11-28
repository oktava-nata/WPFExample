using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Shared.Managers
{
    public class BaseBackgroundWorker
    {
        Action OnSuccessfull { get; set; }
        Action OnFailed { get; set; }
        Action OnCanceled { get; set; }
        Action<int> OnProgressChanged { get; set; }

        public delegate bool LongOperationHandler(DoWorkEventArgs e);
        LongOperationHandler LongOperation { get; set; }
        BackgroundWorker _BackgroundWorker;

        public BaseBackgroundWorker(LongOperationHandler longOperation, Action onSuccessfull, Action onFailed)
        {
            _BackgroundWorker = new BackgroundWorker();

            OnSuccessfull = onSuccessfull;
            OnFailed = onFailed;
            LongOperation = longOperation;

            _BackgroundWorker.WorkerSupportsCancellation = true;
            _BackgroundWorker.DoWork += new DoWorkEventHandler(backgroundWorker_DoWork);
            _BackgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker_RunWorkerCompleted);            
        }

        public BaseBackgroundWorker(LongOperationHandler longOperation, Action onSuccessfull, Action onFailed, Action onCanceled):
            this(longOperation, onSuccessfull, onFailed)
        {
            OnCanceled = onCanceled;
        }

        public BaseBackgroundWorker(LongOperationHandler longOperation, Action onSuccessfull, Action onFailed, Action onCanceled, Action<int> onProgressChanged) :
            this(longOperation, onSuccessfull, onFailed, onCanceled)
        {
            _BackgroundWorker.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker_ProgressChanged);
            _BackgroundWorker.WorkerReportsProgress = true;
            OnProgressChanged = onProgressChanged;
        }

        public bool IsBusy() { return _BackgroundWorker.IsBusy; }

        public bool Abort()
        {
            if (_BackgroundWorker.IsBusy)
            {
                _BackgroundWorker.CancelAsync();
                return true;
            }
            return false;
        }

        public void RunLongOperation_Async()
        {
            _BackgroundWorker.RunWorkerAsync();
        }

        public bool CancellationPending()
        {
            return _BackgroundWorker.CancellationPending;
        }        

        void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {            
            e.Result = LongOperation == null ? false : LongOperation(e);

            if (_BackgroundWorker.CancellationPending)
            {
                e.Cancel = true;
            }
        }

        void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //метод срабатывает даже если закрыть окно, поэтому в случае отмены (т.е. закрыия окна) не выполняем это действие
            if (!e.Cancelled)
            {
                if ((bool)e.Result)
                {
                    if (OnSuccessfull != null)
                        OnSuccessfull();
                }
                else
                {
                    if (OnFailed != null)
                        OnFailed();
                }
            }
            else
                if (OnCanceled != null)
                    OnCanceled();
        }

        private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (OnProgressChanged != null)
                OnProgressChanged(e.ProgressPercentage);            
        }

        public void ReportProgress(int progress)
        {
            if (_BackgroundWorker.WorkerReportsProgress)
                _BackgroundWorker.ReportProgress(Math.Min(progress, 100));
        }
    }

}
