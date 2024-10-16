<a name='assembly'></a>
# JMWToolkit

## Contents

- [AsyncLock](#T-JMWToolkit-AsyncLock 'JMWToolkit.AsyncLock')
  - [#ctor(initiallyLocked)](#M-JMWToolkit-AsyncLock-#ctor-System-Boolean- 'JMWToolkit.AsyncLock.#ctor(System.Boolean)')
  - [Release()](#M-JMWToolkit-AsyncLock-Release 'JMWToolkit.AsyncLock.Release')
  - [Wait()](#M-JMWToolkit-AsyncLock-Wait 'JMWToolkit.AsyncLock.Wait')
  - [Wait(timeout)](#M-JMWToolkit-AsyncLock-Wait-System-TimeSpan- 'JMWToolkit.AsyncLock.Wait(System.TimeSpan)')
- [AsyncLockHelper](#T-JMWToolkit-AsyncLockHelper 'JMWToolkit.AsyncLockHelper')
  - [#ctor(asyncLock,wait)](#M-JMWToolkit-AsyncLockHelper-#ctor-JMWToolkit-AsyncLock,System-Boolean- 'JMWToolkit.AsyncLockHelper.#ctor(JMWToolkit.AsyncLock,System.Boolean)')
  - [CreateAsyncLockHelperAsync(asyncLock)](#M-JMWToolkit-AsyncLockHelper-CreateAsyncLockHelperAsync-JMWToolkit-AsyncLock- 'JMWToolkit.AsyncLockHelper.CreateAsyncLockHelperAsync(JMWToolkit.AsyncLock)')
  - [Dispose(disposing)](#M-JMWToolkit-AsyncLockHelper-Dispose-System-Boolean- 'JMWToolkit.AsyncLockHelper.Dispose(System.Boolean)')
  - [Dispose()](#M-JMWToolkit-AsyncLockHelper-Dispose 'JMWToolkit.AsyncLockHelper.Dispose')
  - [Release()](#M-JMWToolkit-AsyncLockHelper-Release 'JMWToolkit.AsyncLockHelper.Release')
  - [Wait()](#M-JMWToolkit-AsyncLockHelper-Wait 'JMWToolkit.AsyncLockHelper.Wait')
  - [Wait(timeout)](#M-JMWToolkit-AsyncLockHelper-Wait-System-TimeSpan- 'JMWToolkit.AsyncLockHelper.Wait(System.TimeSpan)')
- [MutexHelper](#T-JMWToolkit-MutexHelper 'JMWToolkit.MutexHelper')
  - [#ctor(mutex,wait)](#M-JMWToolkit-MutexHelper-#ctor-System-Threading-Mutex,System-Boolean- 'JMWToolkit.MutexHelper.#ctor(System.Threading.Mutex,System.Boolean)')
  - [Dispose(disposing)](#M-JMWToolkit-MutexHelper-Dispose-System-Boolean- 'JMWToolkit.MutexHelper.Dispose(System.Boolean)')
  - [Dispose()](#M-JMWToolkit-MutexHelper-Dispose 'JMWToolkit.MutexHelper.Dispose')
  - [Release()](#M-JMWToolkit-MutexHelper-Release 'JMWToolkit.MutexHelper.Release')
  - [Wait()](#M-JMWToolkit-MutexHelper-Wait 'JMWToolkit.MutexHelper.Wait')
  - [Wait(timeout)](#M-JMWToolkit-MutexHelper-Wait-System-TimeSpan- 'JMWToolkit.MutexHelper.Wait(System.TimeSpan)')
- [SemaphoreSlimHelper](#T-JMWToolkit-SemaphoreSlimHelper 'JMWToolkit.SemaphoreSlimHelper')
  - [#ctor(semaphore)](#M-JMWToolkit-SemaphoreSlimHelper-#ctor-System-Threading-SemaphoreSlim- 'JMWToolkit.SemaphoreSlimHelper.#ctor(System.Threading.SemaphoreSlim)')
  - [Dispose()](#M-JMWToolkit-SemaphoreSlimHelper-Dispose 'JMWToolkit.SemaphoreSlimHelper.Dispose')
  - [Dispose(disposing)](#M-JMWToolkit-SemaphoreSlimHelper-Dispose-System-Boolean- 'JMWToolkit.SemaphoreSlimHelper.Dispose(System.Boolean)')

<a name='T-JMWToolkit-AsyncLock'></a>
## AsyncLock `type`

##### Namespace

JMWToolkit

##### Summary

This class can be used as an AsyncLock. The caller calls AsyncLock.Wait() and then when they
are done they call AsyncLock.Release() to release there hold on the lock. The calls do not
need to be made on the same thread.

<a name='M-JMWToolkit-AsyncLock-#ctor-System-Boolean-'></a>
### #ctor(initiallyLocked) `constructor`

##### Summary

Initializes the AsyncLock class.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| initiallyLocked | [System.Boolean](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Boolean 'System.Boolean') | Will acquire the lock if true. |

<a name='M-JMWToolkit-AsyncLock-Release'></a>
### Release() `method`

##### Summary

Releases the held lock.

##### Parameters

This method has no parameters.

##### Exceptions

| Name | Description |
| ---- | ----------- |
| [System.InvalidOperationException](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.InvalidOperationException 'System.InvalidOperationException') | The lock is not currently being held. |

<a name='M-JMWToolkit-AsyncLock-Wait'></a>
### Wait() `method`

##### Summary

Blocks the current thread until the lock becomes available.

##### Returns

true if the lock is obtained and false if not.

##### Parameters

This method has no parameters.

<a name='M-JMWToolkit-AsyncLock-Wait-System-TimeSpan-'></a>
### Wait(timeout) `method`

##### Summary

Waits the specified amount of time for the lock to be acquired.

##### Returns

True if lock is acquired, false if not.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| timeout | [System.TimeSpan](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.TimeSpan 'System.TimeSpan') | How long to wait for the lock before returning. |

##### Exceptions

| Name | Description |
| ---- | ----------- |
| [System.ArgumentOutOfRangeException](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.ArgumentOutOfRangeException 'System.ArgumentOutOfRangeException') |  |

<a name='T-JMWToolkit-AsyncLockHelper'></a>
## AsyncLockHelper `type`

##### Namespace

JMWToolkit

##### Summary

Helper class to manage the hold on an AsyncLock and make sure it gets released properly.

<a name='M-JMWToolkit-AsyncLockHelper-#ctor-JMWToolkit-AsyncLock,System-Boolean-'></a>
### #ctor(asyncLock,wait) `constructor`

##### Summary

Initializes the AsyncLockHelper.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| asyncLock | [JMWToolkit.AsyncLock](#T-JMWToolkit-AsyncLock 'JMWToolkit.AsyncLock') | The AsyncLock to be managed. |
| wait | [System.Boolean](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Boolean 'System.Boolean') | Whether or not the lock needs to be acquired. |

<a name='M-JMWToolkit-AsyncLockHelper-CreateAsyncLockHelperAsync-JMWToolkit-AsyncLock-'></a>
### CreateAsyncLockHelperAsync(asyncLock) `method`

##### Summary

Asynchronous routine to create an AsyncLockHelper.

##### Returns

The AsyncLockHelper with the AsyncLock locked.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| asyncLock | [JMWToolkit.AsyncLock](#T-JMWToolkit-AsyncLock 'JMWToolkit.AsyncLock') | The AsyncLock to be managed. |

<a name='M-JMWToolkit-AsyncLockHelper-Dispose-System-Boolean-'></a>
### Dispose(disposing) `method`

##### Summary

Releases the AsyncLock if it is owned.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| disposing | [System.Boolean](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Boolean 'System.Boolean') |  |

<a name='M-JMWToolkit-AsyncLockHelper-Dispose'></a>
### Dispose() `method`

##### Summary

Releases the AsyncLock if it is owned.

##### Parameters

This method has no parameters.

<a name='M-JMWToolkit-AsyncLockHelper-Release'></a>
### Release() `method`

##### Summary

Releases the lock.

##### Parameters

This method has no parameters.

##### Exceptions

| Name | Description |
| ---- | ----------- |
| [System.InvalidOperationException](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.InvalidOperationException 'System.InvalidOperationException') | The lock was not acquired by this helper. |

<a name='M-JMWToolkit-AsyncLockHelper-Wait'></a>
### Wait() `method`

##### Summary

Blocks the current thread until the lock is acquired.

##### Returns

true if the lock was acquired. False otherwise.

##### Parameters

This method has no parameters.

##### Exceptions

| Name | Description |
| ---- | ----------- |
| [System.InvalidOperationException](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.InvalidOperationException 'System.InvalidOperationException') | The lock is already locked by this helper. |

<a name='M-JMWToolkit-AsyncLockHelper-Wait-System-TimeSpan-'></a>
### Wait(timeout) `method`

##### Summary

Blocks the current thread until the lock is acquired, or the timeout is exceeded.

##### Returns



##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| timeout | [System.TimeSpan](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.TimeSpan 'System.TimeSpan') |  |

##### Exceptions

| Name | Description |
| ---- | ----------- |
| [System.InvalidOperationException](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.InvalidOperationException 'System.InvalidOperationException') |  |

<a name='T-JMWToolkit-MutexHelper'></a>
## MutexHelper `type`

##### Namespace

JMWToolkit

##### Summary

This class is used to help with making sure ReleaseMutex is called when the scope of
the method where the mutex is held exits. This is super convenient for a routine which
grabs the mutex, does some work and then releases it:

Mutex _mutex;
using (new MutexHelper(_mutex))
{
    // Do some work here that requires the mutex
}

ReleaseMutex will be called when the using statement exits.

<a name='M-JMWToolkit-MutexHelper-#ctor-System-Threading-Mutex,System-Boolean-'></a>
### #ctor(mutex,wait) `constructor`

##### Summary

Initialize the MutexHelper with the current mutex.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| mutex | [System.Threading.Mutex](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Threading.Mutex 'System.Threading.Mutex') | The mutex to be managed. |
| wait | [System.Boolean](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Boolean 'System.Boolean') | Waits for the mutex to be locked. |

<a name='M-JMWToolkit-MutexHelper-Dispose-System-Boolean-'></a>
### Dispose(disposing) `method`

##### Summary

Releases the lock if held.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| disposing | [System.Boolean](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Boolean 'System.Boolean') |  |

<a name='M-JMWToolkit-MutexHelper-Dispose'></a>
### Dispose() `method`

##### Summary

Releases the lock if held.

##### Parameters

This method has no parameters.

<a name='M-JMWToolkit-MutexHelper-Release'></a>
### Release() `method`

##### Summary

Releases the mutex.

##### Parameters

This method has no parameters.

##### Exceptions

| Name | Description |
| ---- | ----------- |
| [System.InvalidOperationException](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.InvalidOperationException 'System.InvalidOperationException') | This MutexHelper does not own the mutex. |

<a name='M-JMWToolkit-MutexHelper-Wait'></a>
### Wait() `method`

##### Summary

Waits for the Mutex to be acquired.

##### Returns

true if the mutex is held and false if not.

##### Parameters

This method has no parameters.

##### Exceptions

| Name | Description |
| ---- | ----------- |
| [System.InvalidOperationException](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.InvalidOperationException 'System.InvalidOperationException') | The mutex is already owned by this helper. |

<a name='M-JMWToolkit-MutexHelper-Wait-System-TimeSpan-'></a>
### Wait(timeout) `method`

##### Summary

Waits the given amount of time for the mutex to be acquired.

##### Returns

true if the lock is acquired, false if not.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| timeout | [System.TimeSpan](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.TimeSpan 'System.TimeSpan') | The amount of time to wait for the mutex. |

##### Exceptions

| Name | Description |
| ---- | ----------- |
| [System.InvalidOperationException](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.InvalidOperationException 'System.InvalidOperationException') | This helper already owns the mutex. |

<a name='T-JMWToolkit-SemaphoreSlimHelper'></a>
## SemaphoreSlimHelper `type`

##### Namespace

JMWToolkit

##### Summary

Helper class for locking and releasing a SemaphoreSlim object.

<a name='M-JMWToolkit-SemaphoreSlimHelper-#ctor-System-Threading-SemaphoreSlim-'></a>
### #ctor(semaphore) `constructor`

##### Summary

Initializes the SemaphoreSlimHelper.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| semaphore | [System.Threading.SemaphoreSlim](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Threading.SemaphoreSlim 'System.Threading.SemaphoreSlim') | The Semaphore to be managed. |

<a name='M-JMWToolkit-SemaphoreSlimHelper-Dispose'></a>
### Dispose() `method`

##### Summary

Releases the semaphore if owned.

##### Parameters

This method has no parameters.

<a name='M-JMWToolkit-SemaphoreSlimHelper-Dispose-System-Boolean-'></a>
### Dispose(disposing) `method`

##### Summary

Releases the semaphore if held.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| disposing | [System.Boolean](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Boolean 'System.Boolean') |  |
