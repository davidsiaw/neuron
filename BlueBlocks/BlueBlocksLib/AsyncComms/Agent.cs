using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace BlueBlocksLib.AsyncComms
{
    public enum NextAction
    {
        Finish,
        WaitForNextMessage,
    }

    public class Agent<T>
    {

        Thread t;
        IChannel<T> messageQueue = new MessageQueue<T>();
        Action initAction;
        Func<NextAction, T> taskAction;
        Action exitAction;

        /// <summary>
        /// Use this constructor to perform delegation if you do not wish
        /// to subclass Agent to create your own agent to define actions
        /// </summary>
        /// <param name="initAction"></param>
        /// <param name="exitAction"></param>
        /// <param name="taskAction"></param>
        public Agent(Action initAction, Action exitAction, Func<NextAction, T> taskAction)
            : this()
        {
            this.initAction = initAction;
            this.taskAction = taskAction;
            this.exitAction = exitAction;
        }

        public Agent(Action initAction, Action exitAction, Func<NextAction, T> taskAction, IChannel<T> messageQueue)
            : this(initAction, exitAction, taskAction)
        {
            this.messageQueue = messageQueue;
        }

        /// <summary>
        /// If you subclass the agent, use this constructor and override
        /// the InitAction, ExitAction and TaskAction methods. Subclass
        /// this Agent if this agent needs to maintain complex state
        /// and needs to be instatiated at different parts of the program
        /// </summary>
        protected Agent()
        {
            initAction = InitAction;
            exitAction = ExitAction;
            taskAction = TaskAction;

            t = new Thread(new ParameterizedThreadStart(mq =>
            {
                initAction();
                T item;
                NextAction na = NextAction.WaitForNextMessage;
                do {
                    item = (mq as MessageQueue<T>).Receive();
                    na = taskAction(item);
                } while (na != NextAction.Finish);
                exitAction();
            }));
			t.IsBackground = true;
        }

        protected Agent(MessageQueue<T> messageQueue)
            : this()
        {
            this.messageQueue = messageQueue;
        }

        /// <summary>
        /// Tasks to perform before the message loop starts
        /// </summary>
        protected virtual void InitAction()
        {
        }

        /// <summary>
        /// Tasks to perform just before the thread exits
        /// </summary>
        protected virtual void ExitAction()
        {
        }

        /// <summary>
        /// Do some computation and say if the thread should continue running
        /// </summary>
        /// <param name="item">The message passed to the thread</param>
        /// <returns>True if the thread should continue running</returns>
        protected virtual NextAction TaskAction(T item)
        {
            return NextAction.Finish;
        }

        /// <summary>
        /// Start the agent
        /// </summary>
        public void Start()
        {
            t.Start(messageQueue);
        }

        /// <summary>
        /// Send the agent a message
        /// </summary>
        /// <param name="message">The message to send the agent</param>
        public void SendMessage(T message)
        {
            if (!t.IsAlive)
            {
                throw new InvalidOperationException("Agent not started");
            }
            messageQueue.Send(message);
        }

		public int MessageQueueLength {
			get {
				return messageQueue.Count();
			}
		}
    }
}
