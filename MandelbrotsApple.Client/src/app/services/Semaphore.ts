/**
 * From repository: https://gist.github.com/cahilfoley/4b1b2f3fa9e2f9652ee1d8501443b5ca
 */

/**
 * A lock that is granted when calling [[Semaphore.acquire]].
 */
type Lock = {
    release: () => void;
};

/**
 * A task that has been scheduled with a [[Semaphore]] but not yet started.
 */
type WaitingPromise = {
    resolve: (lock: Lock) => void;
    reject: (err?: Error) => void;
};

/**
 * A [[Semaphore]] is a tool that is used to control concurrent access to a common resource. This implementation
 * is used to apply a max-parallelism threshold.
 */
export class Semaphore {
    private running = 0;
    private waiting: WaitingPromise[] = [];
    private max = 1;

    constructor(private label: string) {}

    /**
     * Acquire a lock on the target resource.
     *
     * ! Returns a function to release the lock, it is critical that this function is called when the task is finished with the resource.
     */
    acquire = (): Promise<Lock> => {
        if (this.running < this.max) {
            this.running++;
            return Promise.resolve({ release: this.release });
        }

        return new Promise<Lock>((resolve, reject) => {
            this.waiting.push({ resolve, reject });
        });
    };

    /**
     * Purge all waiting tasks from the [[Semaphore]]
     */
    purge = () => {
        this.waiting.forEach((task) => {
            task.reject(
                new Error(
                    'The semaphore was purged and as a result this task has been cancelled'
                )
            );
        });

        this.running = 0;
        this.waiting = [];
    };

    /**
     * Allows the next task to start, if there are any waiting.
     */
    private take = () => {
        if (this.waiting.length > 0 && this.running < this.max) {
            this.running++;

            // Get the next task from the queue
            const task = this.waiting.shift();

            // Resolve the promise to allow it to start, provide a release function
            if (task !== undefined) task.resolve({ release: this.release });
        }
    };

    /**
     * Releases a lock held by a task. This function is returned from the acquire function.
     */
    private release = () => {
        this.running--;
        this.take();
    };
}
