import { MandelbrotPosition } from './mandelbrot-position';

export class MandelbrotSize {
    constructor(public Min: MandelbrotPosition, public Max: MandelbrotPosition) {}

    get Middle(): MandelbrotPosition {
        return new MandelbrotPosition((this.Min.X + this.Max.X)/ 2.0, (this.Min.Y + this.Max.Y)/ 2.0);
    }
};
