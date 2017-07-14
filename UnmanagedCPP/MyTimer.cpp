#include "stdafx.h"
#include "MyTimer.h"

#define TIMER_CYCLE 10 // 10 seconds

CMyTimer::CMyTimer()
	:m_pfnCallBack(NULL), m_bStop(true)
{
}


CMyTimer::~CMyTimer()
{
}

CMyTimer & CMyTimer::GetInstance()
{
	static CMyTimer timer;
	return timer;
}

inline void CMyTimer::Lock()
{
	m_locker.lock();
}

void CMyTimer::Unlock()
{
	m_locker.unlock();
}

void CMyTimer::Start(CallBack pfnCallBack)
{
	Stop();
	Lock();
	{
		m_bStop = false;
		m_pfnCallBack = pfnCallBack;
		m_thread = thread(CMyTimer::ThreadFunc, this);
	}
	Unlock();
}

void CMyTimer::Stop()
{
	Lock();
	{
		if (m_bStop == true)
		{
			Unlock();
			return;
		}
		m_bStop = true;
	}
	Unlock();
	m_thread.join();
}

DWORD CMyTimer::ThreadFunc(LPVOID lpvParam)
{
	CMyTimer *pObj = (CMyTimer *)lpvParam;
	if (!pObj)
		return 1;
	return pObj->Thread();
}

DWORD CMyTimer::Thread()
{
	time_t nTimeStamp = 0;
	CALLBACK_FIRST_PARAM param;
	while (true)
	{
		Lock();
		{
			if (m_bStop == true || m_pfnCallBack == NULL)
			{
				Unlock();
				break;
			}
			time_t nCurrentTime = time(NULL);
			if (abs(nCurrentTime - nTimeStamp) >= TIMER_CYCLE)
			{
				nTimeStamp = nCurrentTime;
				m_pfnCallBack(&param, &nCurrentTime);
				Sleep(10);
			}
		}
		Unlock();
	}
	return 0;
}
