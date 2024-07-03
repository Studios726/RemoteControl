using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RemoteControl.Event
{
    public class EventManager : Singleton<EventManager>
    {
        private Dictionary<string, EventHandler> handlerDic = new Dictionary<string, EventHandler>();

        /// <summary>
        /// 添加事件监听
        /// </summary>
        /// <param name="eventName">事件名</param>
        /// <param name="handler">处理函数</param>
        public void AddListener(string eventName, EventHandler handler)
        {
            if (handlerDic.ContainsKey(eventName))
            {
                handlerDic[eventName] += handler;
            }
            else
            {
                handlerDic.Add(eventName, handler);
            }
        }

        /// <summary>
        /// 移除事件监听
        /// </summary>
        /// <param name="eventName">事件名</param>
        /// <param name="handler">处理函数</param>
        public void RemoveListener(string eventName, EventHandler handler)
        {
            if (handlerDic.ContainsKey(eventName))
            {
                handlerDic[eventName] -= handler;
            }
        }

        /// <summary>
        /// 触发事件（无参数）
        /// </summary>
        /// <param name="eventName">事件名</param>
        /// <param name="sender">触发源</param>
        public void TriggerEvent(string eventName, object sender)
        {
            if (handlerDic.ContainsKey(eventName))
                handlerDic[eventName]?.Invoke(sender, EventArgs.Empty);
        }

        /// <summary>
        /// 触发事件（有参数）
        /// </summary>
        /// <param name="eventName">事件名</param>
        /// <param name="sender">触发源</param>
        /// <param name="args">事件参数</param>
        public void TriggerEvent(string eventName, object sender, EventArgs args)
        {
            if (handlerDic.ContainsKey(eventName))
                handlerDic[eventName]?.Invoke(sender, args);
        }

        /// <summary>
        /// 清空所有事件
        /// </summary>
        public void Clear()
        {
            handlerDic.Clear();
        }
    }
}
