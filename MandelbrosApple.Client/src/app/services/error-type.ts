export enum ErrorType {
  None = 0,
  ParameterIsNull = 1,
  WidthIsLessThanOnePixelError = 2,
  HeightIsLessThanOnePixelError = 3,
  WidthIsGreaterThan10000PixelError = 4,
  HeightIsGreaterThan10000PixelError = 5,
  XMinIsGreaterThanXMaxError = 6,
  YMinIsGreaterThanYMaxError = 7,
  XMinAndXMaxDifferenceToSmall = 8,
  YMinAndYMaxDifferenceToSmall = 9,
  IterationLessOrEqualThanZeroError = 10,
  IterationGreaterThanThousandError = 11,
}
