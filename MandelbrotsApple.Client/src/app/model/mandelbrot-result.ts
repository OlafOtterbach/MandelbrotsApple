import { MandelbrotSize } from "./mandelbrot-size";
import { ErrorType } from "./error-type";

export interface MandelbrotResult {
  imageData: string,
  mandelbrotSize: MandelbrotSize,
  errors: ErrorType[],
  hasErrors: boolean
}
