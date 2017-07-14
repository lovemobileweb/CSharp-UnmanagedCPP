#pragma once

#include <thread>
#include <mutex>

using namespace std;

#pragma pack(push)
#pragma pack(1)
typedef struct
{
	int member1;
	int member2;
}CALLBACK_FIRST_PARAM;
#pragma pack(pop)

typedef int(__stdcall * CallBack)(CALLBACK_FIRST_PARAM *pParam, time_t *pCurrentTime);

class CMyTimer
{
public:
	CMyTimer();
	~CMyTimer();
	static CMyTimer &GetInstance();
	void Start(CallBack pfnCallBack);
	void Stop();

private:
	bool m_bStop;
	CallBack m_pfnCallBack;
	thread m_thread;
	mutex m_locker;

	void Lock();
	void Unlock();

	static DWORD WINAPI ThreadFunc(LPVOID lpvParam);
	DWORD Thread();
};

