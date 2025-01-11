import { ImageSize } from "./image-size";
import { MandelbrotSize } from "./mandelbrot-size";

export class MandelbrotParameter {
  constructor(
    public ImageSize: ImageSize,
    public CurrentMandelbrotSize: MandelbrotSize,
    public MaxIterations: number) {
  }
}
